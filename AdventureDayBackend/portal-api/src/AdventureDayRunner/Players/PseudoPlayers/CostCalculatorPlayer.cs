using Microsoft.Azure.Management.Compute.Fluent;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using team_management_api.Data;

namespace AdventureDayRunner.Players.PseudoPlayers
{
    public class CostCalculatorPlayer : PseudoPlayerBase
    {
        private readonly IConfiguration _configuration;
        private readonly double _azureCostScaleFactor;

        public CostCalculatorPlayer(IConfiguration configuration, Team team, TimeSpan httpTimeout) : base(configuration, team, httpTimeout)
        {
            _configuration = configuration;
           _azureCostScaleFactor = _configuration.GetValue("AzureCostScaleFactor", 1000.0); 
        }

        public override string Name => "Dagobert";

        protected override async Task<MatchReport> ExecuteAction(Team team, HttpClient httpClient, CancellationToken cancellationToken)
        {
            var azureCredentials = new AzureCredentials(new ServicePrincipalLoginInformation
            {
                ClientId = _configuration.GetValue<string>("AzureSPClientId"),
                ClientSecret = _configuration.GetValue<string>("AzureSPClientSecret")
            }, team.TenantId.ToString(), AzureEnvironment.AzureGlobalCloud);

            var azure = Azure
                .Configure()
                .Authenticate(azureCredentials)
                .WithSubscription(team.SubscriptionId.ToString());

            long aksCosts = await GetAksCostAsync(azure);
            long sqlCosts = await GetSqlCostAsync(azure);

            return MatchReport.FromCostCalculator((int)(aksCosts + sqlCosts));
        }

        private async Task<long> GetSqlCostAsync(IAzure azure)
        {
            long sqlCost = 0;

            foreach (var sqlServer in await azure.SqlServers.ListAsync())
            {
                foreach (var database in await sqlServer.Databases.ListAsync())
                {
                    // Azure Policy ensure only GeneralPurpose is allowed
                    var serviceLevel = database.ServiceLevelObjective.ToString(); // GP_S_Gen5_1 etc.
                    if (serviceLevel.StartsWith("GP_S_Gen5"))
                    {
                        var appCpuBilledMetric = (await azure.MetricDefinitions.ListByResourceAsync(database.Id))
                            .SingleOrDefault(_ => _.Name.Value == "app_cpu_billed");

                        var now = DateTime.Now.ToUniversalTime();

                        var metricCollection = await appCpuBilledMetric
                                .DefineQuery()
                                .StartingFrom(now.AddMinutes(-5))
                                .EndsBefore(now)
                                .WithInterval(TimeSpan.FromMinutes(1))
                                .ExecuteAsync();

                        // https://docs.microsoft.com/en-us/azure/azure-sql/database/serverless-tier-overview#metrics
                        // Unit: vCore seconds
                        var lastMinuteAppCPUBilled = metricCollection.Metrics
                            .Single()
                            .Timeseries
                            .SelectMany(_ => _.Data)
                            .OrderByDescending(_ => _.TimeStamp)
                            .First()
                            .Total;

                        // COMPUTE COST / VCORE / SECOND: 0.000134 EUR
                        // scaleFactor just for having bigger numbers
                        if (lastMinuteAppCPUBilled != null)
                            sqlCost += (int) Math.Round(
                                0.000134 * lastMinuteAppCPUBilled.Value * _azureCostScaleFactor,
                                MidpointRounding.AwayFromZero);
                    }
                    else if (serviceLevel.StartsWith("GP_Gen5"))
                    {
                        var vCoreCount = int.Parse(serviceLevel.Replace("GP_Gen5_", ""));

                        // Cost per vCore (in EUR, month) 167.77
                        // * scaleFactor just for having bigger numbers
                        sqlCost += (int)Math.Round(vCoreCount * 167.77 / 31 / 24 / 60 * _azureCostScaleFactor, MidpointRounding.AwayFromZero);
                    }
                }
            }

            return sqlCost;
        }

        private static async Task<long> GetAksCostAsync(IAzure azure)
        {
            long aksCosts = 0;
            foreach (var kubernetesCluster in await azure.KubernetesClusters.ListAsync())
            {
                var computeSkus = azure.ComputeSkus
                        .ListbyRegionAndResourceType(kubernetesCluster.Region, ComputeResourceType.VirtualMachines);

                foreach (var agentPool in kubernetesCluster.AgentPools)
                {
                    var vmCount = agentPool.Value.Count;
                    var vmSize = agentPool.Value.VMSize;

                    var computeSkusCosts = computeSkus
                        .Single(_ => _.VirtualMachineSizeType.ToString() == vmSize.ToString())
                        .Costs;

                    var vmCost = computeSkusCosts
                        .Sum(_ => _.Quantity.GetValueOrDefault());

                    aksCosts += vmCount * vmCost;
                }
            }

            return aksCosts;
        }
    }
}
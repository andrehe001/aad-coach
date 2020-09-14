using Microsoft.Azure.Management.Compute.Fluent;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using team_management_api.Data;

namespace AdventureDayRunner.Players.PseudoPlayers
{
    public class CostCalculatorPlayer : PseudoPlayerBase
    {
        public CostCalculatorPlayer(Team team, TimeSpan httpTimeout) : base(team, httpTimeout)
        {
        }

        public override string Name => "Dagobert";

        protected override async Task<MatchReport> ExecuteAction(Team team, HttpClient httpClient, CancellationToken cancellationToken)
        {
            var azureCredentials = new AzureCredentials(new ServicePrincipalLoginInformation
            {
                ClientId = "b9415f6c-a502-4cae-b1f2c81",
                ClientSecret = "HF6kR8_a7xH7MdQ.kf0UU3sVjeZ"
            }, "72f988bf-86f1-41af-91ab-2d7cd011db47", AzureEnvironment.AzureGlobalCloud);

            var azure = Azure
                .Configure()
                .Authenticate(azureCredentials)
                .WithSubscription("73119864-58f7-4cb3-b1e5-98300bcc2557");

            long aksCosts = await GetAksCostAsync(azure);
            long sqlCosts = await GetSqlCostAsync(azure);

            return MatchReport.FromCostCalculator((int)(aksCosts + sqlCosts), 1);
        }

        private async Task<long> GetSqlCostAsync(IAzure azure)
        {
            long sqlCost = 0;

            foreach (var sqlServer in await azure.SqlServers.ListAsync())
            {
                foreach (var database in await sqlServer.Databases.ListAsync())
                {
                    // TODO: Only allow GeneralPurpose
                    //var test1 = database.Edition == DatabaseEdition.GeneralPurpose;

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
                        // *1000 just for having bigger numbers
                        sqlCost += (int)Math.Round((decimal)(0.000134 * lastMinuteAppCPUBilled * 10000), MidpointRounding.AwayFromZero);
                    }
                    else if (serviceLevel.StartsWith("GP_Gen5"))
                    {
                        var vCoreCount = int.Parse(serviceLevel.Replace("GP_Gen5_", ""));

                        // Cost per vCore (in EUR, month) 167.77
                        // *1000 just for having bigger numbers
                        sqlCost += (int)Math.Round(vCoreCount * 167.77 / 31 / 24 / 60 * 10000, MidpointRounding.AwayFromZero);
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
                foreach (var agentPool in kubernetesCluster.AgentPools)
                {
                    var vmCount = agentPool.Value.Count;
                    var vmSize = agentPool.Value.VMSize;

                    var computeSkusCosts = azure.ComputeSkus
                        .ListbyRegionAndResourceType(kubernetesCluster.Region, ComputeResourceType.VirtualMachines)
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
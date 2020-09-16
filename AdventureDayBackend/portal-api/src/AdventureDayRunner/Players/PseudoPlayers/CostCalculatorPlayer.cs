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
           _azureCostScaleFactor = _configuration.GetValue("AzureCostScaleFactor", 1); 
        }

        public override string Name => "Dagobert";

        protected override async Task<MatchReport> ExecuteAction(Team team, HttpClient httpClient, CancellationToken cancellationToken)
        {
            var azureCredentials = new AzureCredentials(new ServicePrincipalLoginInformation
            {
                ClientId = _configuration.GetValue<string>("AzureSPClientId", "a521ec4a-f8fe-4cb0-b54c-93f74a47d88f"),
                ClientSecret = _configuration.GetValue<string>("AzureSPClientSecret", "LhNqATyW_4KIy-AhDr~ic.NR.KDD5yOa9p")
            }, team.TenantId.ToString(), AzureEnvironment.AzureGlobalCloud);

            var azure = Azure
                .Configure()
                .Authenticate(azureCredentials)
                .WithSubscription(team.SubscriptionId.ToString());

            // TODO: Take a look why costs is empty
            //long aksCosts = await GetAksCostAsync(azure);
            long sqlCosts = await GetSqlCostAsync(azure);

            var totalCost = (int)Math.Round(sqlCosts * _azureCostScaleFactor, MidpointRounding.AwayFromZero);
            return MatchReport.FromCostCalculator(totalCost);
        }

        private async Task<long> GetSqlCostAsync(IAzure azure)
        {
            long sqlCost = 0;
            var sqlCostScaleFactor = 265;

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

                        var test = metricCollection.Metrics
                            .Single()
                            .Timeseries
                            .SelectMany(_ => _.Data)
                            .OrderByDescending(_ => _.TimeStamp)
                            .ToList();

                        // COMPUTE COST / VCORE / SECOND: 0.000134 EUR
                        var costPerVCorePerSecond = 0.000134;
                        var usedVCoreSecondsInLastMinute = lastMinuteAppCPUBilled.GetValueOrDefault(); // max 60 * vCoreCount

                        // 1 VCore ganze Minute
                        //var cost = costPerVCorePerSecond * 60 * 265;
                        //2,1306
                        sqlCost += (int)Math.Round(costPerVCorePerSecond * usedVCoreSecondsInLastMinute * sqlCostScaleFactor, MidpointRounding.AwayFromZero);
                    }
                    else if (serviceLevel.StartsWith("GP_Gen5"))
                    {
                        var vCoreCount = int.Parse(serviceLevel.Replace("GP_Gen5_", ""));

                        // Cost per vCore (in EUR, month) 167.77
                        var costPerVCorePerSecond = 0.000063;
                        var usedVCoreSecondsInLastMinute = 60 * vCoreCount;

                        // 1 VCore ganze Minute
                        //var cost = costPerVCorePerSecond * 60 * 265;
                        //1,0017

                        // * scaleFactor just for having bigger numbers
                        sqlCost += (int)Math.Round(costPerVCorePerSecond * usedVCoreSecondsInLastMinute * sqlCostScaleFactor, MidpointRounding.AwayFromZero);
                    }
                }
            }

            return sqlCost;
        }

        //private static async Task<long> GetAksCostAsync(IAzure azure)
        //{
        //    long aksCosts = 0;
        //    foreach (var kubernetesCluster in await azure.KubernetesClusters.ListAsync())
        //    {
        //        var computeSkus = azure.ComputeSkus
        //                .ListbyRegionAndResourceType(kubernetesCluster.Region, ComputeResourceType.VirtualMachines);

        //        foreach (var agentPool in kubernetesCluster.AgentPools)
        //        {
        //            var vmCount = agentPool.Value.Count;
        //            var vmSize = agentPool.Value.VMSize;

        //            var computeSkusCosts = computeSkus
        //                .Single(_ => _.VirtualMachineSizeType.ToString() == vmSize.ToString())
        //                .Costs;

        //            var vmCost = computeSkusCosts
        //                .Sum(_ => _.Quantity.GetValueOrDefault());

        //            aksCosts += vmCount * vmCost;
        //        }
        //    }

        //    return aksCosts;
        //}
    }
}
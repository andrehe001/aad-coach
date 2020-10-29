using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using Serilog;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using AdventureDay.DataModel;
using Microsoft.Extensions.Configuration;

namespace AdventureDay.Runner.Players.PseudoPlayers
{
    public class CostCalculatorPlayer : PseudoPlayerBase
    {

        public class MetricsResponse
        {
            public bool MetricsError { get; set; }
            public long Price { get; set; }
            public int TotalMemory { get; set; }

            public int TotalCores { get; set; }

            public Node[] Nodes { get; set; }
        }

        public class Node
        {
            public long Price { get; set; }
            public int AvailableMemory { get; set; }
            public int AvailableCores { get; set; }
            public int UsedMemory { get; set; }
            public int UsedCores { get; set; }
            public string Type { get; set; }
        }

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
                ClientId = _configuration.GetValue<string>("AzureSPClientId", "e27ebe5d-70bb-4921-978d-7315ed6f0a3d"),
                ClientSecret = _configuration.GetValue<string>("AzureSPClientSecret", "9T.m~b54xKTzNw6-0M-LTE7.XPsNVEF5_B")
            }, team.TenantId.ToString(), AzureEnvironment.AzureGlobalCloud);

            var azure = Azure
                .Configure()
                .Authenticate(azureCredentials)
                .WithSubscription(team.SubscriptionId.ToString());

            // TODO: Take a look why costs is empty
            long aksCosts = await GetAksCostAsync(team, httpClient, cancellationToken);
            long sqlCosts = await GetSqlCostAsync(azure);

            var totalCost = (int)Math.Round(sqlCosts * _azureCostScaleFactor + aksCosts * _azureCostScaleFactor, MidpointRounding.AwayFromZero);
            return MatchReport.FromCostCalculator(totalCost);
        }

        private async Task<long> GetSqlCostAsync(IAzure azure)
        {
            long sqlCost = 0;

            try
            {
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
            }
            catch (Exception ex)
            {
                Log.Error(ex, "CostCalculatorPlayer: GetSqlCostAsync had an issue");
                sqlCost += 1;
            }

            return sqlCost;
        }

        private static async Task<long> GetAksCostAsync(Team team, HttpClient httpClient, CancellationToken cancellationToken)
        {
            long aksCosts = 5; // default penalty for unavailable costs

            if (!string.IsNullOrWhiteSpace(team.GameEngineUri)){
                Uri gameEngineSidecarUri; 

                Log.Debug("Costplayer: Using default game uri");
                var gameEngineSidecarUriBuilder = new UriBuilder(team.GameEngineUri);
                gameEngineSidecarUriBuilder.Port = 80;
                gameEngineSidecarUriBuilder.Path = "Metrics";
                gameEngineSidecarUri = gameEngineSidecarUriBuilder.Uri;

                try
                {
                    var result = await httpClient.GetAsync(gameEngineSidecarUri, cancellationToken: cancellationToken);
                    if (result.IsSuccessStatusCode)
                    {
                        var response = await result.Content.ReadFromJsonAsync<MetricsResponse>(
                            cancellationToken: cancellationToken);

                        if (response != null){
                            if ( response.MetricsError )
                            {
                                aksCosts = 0;
                            }else
                            {
                                aksCosts = response.Price;
                            }
                        }

                        
                    }
                }
                catch (Exception exception)
                {
                    Log.Information($"{nameof(CostCalculatorPlayer)}: Error in in reaching cost endpoint (not counted as Team Error). URI: {gameEngineSidecarUri.ToString()} Team: {team.Name} (ID: {team.Id})");
                    Log.Debug(exception, $"{nameof(CostCalculatorPlayer)}: Error in in reaching cost endpoint (not counted as Team Error). URI: {gameEngineSidecarUri.ToString()} Team: {team.Name} (ID: {team.Id})");
                }
            }

            return aksCosts;
        }
    }
}
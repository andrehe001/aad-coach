using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AdventureDayRunner.Model;
using AdventureDayRunner.Players;
using AdventureDayRunner.Players.PseudoPlayers;
using AdventureDayRunner.Players.RealPlayers;
using AdventureDayRunner.Utils;
using Autofac;
using Microsoft.EntityFrameworkCore;
using Serilog;
using team_management_api.Data;
using team_management_api.Data.Runner;

namespace AdventureDayRunner
{
    public class RunnerEngine
    {
        private readonly int _refreshTimeoutInSeconds = 5;
        private readonly ILifetimeScope _lifetimeScope;

        public RunnerEngine(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
        }

        public async Task Run(CancellationToken cancellationToken)
        {
            RunnerProperties properties;
            List<Team> teams;
            DateTime lastRefresh = DateTime.UtcNow;
            
            using (var lifetimeScope = _lifetimeScope.BeginLifetimeScope())
            {
                var dbContext = lifetimeScope.Resolve<AdventureDayBackendDbContext>();
                properties = await dbContext.RunnerProperties.FirstOrDefaultAsync(_ =>
                    _.Name == RunnerProperties.DefaultRunnerPropertiesName, cancellationToken);
                teams= await dbContext.Teams.ToListAsync(cancellationToken);
            }
            
            do
            {
                DateTime now = DateTime.UtcNow;
                TimeSpan refreshDelta = now - lastRefresh;
                
                var currentPhase = properties.CurrentPhase;
                var phaseConfiguration = properties.PhaseConfigurations[currentPhase];
                
                if (properties.RunnerStatus == RunnerStatus.Started)
                {
                    Log.Information(
                        $"--- New Wave --- Phase: {currentPhase.ToString()} Latency: {phaseConfiguration.RequestExecutorLatencyMillis} (Config Refresh: {refreshDelta.Seconds} sec ago.)");
                    
                    // Fire forget match requests for all configured players.
                    foreach (var team in teams)
                    {
                        foreach (var playerType in phaseConfiguration.PlayerTypes)
                        {
                            InvokePlayerWithFireAndForget(playerType, team, cancellationToken);
                        }
                    }
                    
                    // Wait with the next wave of requests.
                    await Task.Delay(phaseConfiguration.RequestExecutorLatencyMillis, cancellationToken);
                }
                else
                {
                    Log.Information($"Status: {properties.RunnerStatus.ToString()} (retry in 5secs)");
                    await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
                }


                if (refreshDelta.Seconds > _refreshTimeoutInSeconds)
                {
                    Log.Information("Refreshing Configuration...");
                    lastRefresh = DateTime.UtcNow;
                    using (var lifetimeScope = _lifetimeScope.BeginLifetimeScope())
                    {
                        var dbContext = lifetimeScope.Resolve<AdventureDayBackendDbContext>();
                        properties = await dbContext.RunnerProperties.FirstOrDefaultAsync(_ =>
                            _.Name == RunnerProperties.DefaultRunnerPropertiesName, cancellationToken);
                        teams= await dbContext.Teams.ToListAsync(cancellationToken);
                    }
                }
            } while (!cancellationToken.IsCancellationRequested);
        }

        private void InvokePlayerWithFireAndForget(
            PlayerType playerType,
            Team team,
            CancellationToken cancellationToken)
        {
            // TODO make http timeout configurable.
            var httpTimeout = TimeSpan.FromSeconds(5);
            
            Task.Run(async () =>
            {
                MatchReport report = null;
                try
                {
                    Log.Debug($"Team {team.Name} vs. Player {playerType.ToString()}");
                    var player = CreatePlayerFromType(playerType, team, httpTimeout);

                    report = await player.Play(cancellationToken);
                }
                catch (TaskCanceledException exception)
                {
                    // TODO log reason.
                    if (!cancellationToken.IsCancellationRequested)
                    {
                        Log.Error("HTTP Timeout.");
                        report = MatchReport.FromError("HTTP Timeout");
                    }
                    else
                    {
                        Log.Debug(exception: exception, "TaskCanceledException | No Http Timeout detected.");
                    }
                }
                catch (MatchCanceledException)
                {
                    Log.Debug("Smoorghs cancelled the match.");
                    report = MatchReport.FromCancellation("Smoorghs cancelled the match.");
                }
                catch (Exception exception)
                {
                    // Should rarely occur.
                    Log.Error(exception,
                        $"Issue in Fire and Forget for Team {team.Name} URI: {team.GameEngineUri}");
                    report = MatchReport.FromError("HTTP Timeout");
                }
                
                await using var lifetimeScope = _lifetimeScope.BeginLifetimeScope();
                var dbContext = lifetimeScope.Resolve<AdventureDayBackendDbContext>();
                await dbContext.TeamLogEntries.AddAsync(
                    new TeamLogEntry() {Reason = "Test", ResponeTimeMs = 100, Status = "200", TeamId = team.Id},
                    cancellationToken);
                await dbContext.SaveChangesAsync(cancellationToken);
                 
            }, cancellationToken).Forget();
        }
        
        private static IPlayer CreatePlayerFromType(PlayerType playerType, Team team, TimeSpan httpTimeout)
        {
            IPlayer player = playerType switch
            {
                PlayerType.Pattern => new PatternPlayer(team, httpTimeout),
                PlayerType.Random => new RandomPlayer(team, httpTimeout),
                PlayerType.Fixed => new FixedPlayer(team, httpTimeout),
                PlayerType.Iterative => new IterativePlayer(team, httpTimeout),
                PlayerType.Bet => new BetPlayer(team, httpTimeout),
                PlayerType.CostCalculator => new CostCalculatorPlayer(team, httpTimeout),
                PlayerType.SecurityHack => new SecurityHackPlayer(team, httpTimeout),
                _ => throw new InvalidOperationException("Unexpected enum value.")
            };
            
            return player;
        }
    }
}
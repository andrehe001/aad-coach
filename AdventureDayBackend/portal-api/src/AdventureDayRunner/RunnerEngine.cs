using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AdventureDayRunner.Players;
using AdventureDayRunner.Players.PseudoPlayers;
using AdventureDayRunner.Players.RealPlayers;
using AdventureDayRunner.Utils;
using Autofac;
using EFCore.BulkExtensions;
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
            DateTime lastRefresh;
            List<Team> teams;
            RunnerProperties properties;
            
            (lastRefresh, teams, properties) = await RefreshConfiguration(cancellationToken);
            
            do // until we cancel completely externally
            {
                DateTime now = DateTime.UtcNow;
                TimeSpan refreshDelta = now - lastRefresh;
                
                var currentPhase = properties.CurrentPhase;
                var phaseConfiguration = properties.PhaseConfigurations[currentPhase];
                
                switch (properties.RunnerStatus)
                {
                    case RunnerStatus.Started:
                        Log.Information(
                            $"--- New Wave --- Phase: {currentPhase.ToString()}"
                            + $" Latency: {phaseConfiguration.RequestExecutorLatencyMillis}"
                            + $" (Config Refresh: {refreshDelta.Seconds} sec ago.)");
                        await RunInnerLoop(phaseConfiguration, teams, cancellationToken);
                        break;
                    case RunnerStatus.Stopped:
                        Log.Information(
                            $"Status: {properties.RunnerStatus.ToString()} (retry in 5secs)");
                        await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
                        break;
                    default:
                        throw new InvalidOperationException($"Unexpected RunnerStatus: {properties.RunnerStatus.ToString()}");
                }

                if (refreshDelta.Seconds > _refreshTimeoutInSeconds)
                {
                    (lastRefresh, teams, properties) = await RefreshConfiguration(cancellationToken);
                }
            } while (!cancellationToken.IsCancellationRequested);
        }

        private async Task RunInnerLoop(
            RunnerPhaseConfigurationItem phaseConfiguration, 
            List<Team> teams, 
            CancellationToken cancellationToken)
        {
            // Fire forget match requests for all configured players.
            foreach (var team in teams)
            {
                foreach (var playerType in phaseConfiguration.PlayerTypes)
                {
                    InvokePlayerWithFireAndForget(playerType, team, cancellationToken);
                }
            }

            // Delay the next wave of requests.
            await Task.Delay(phaseConfiguration.RequestExecutorLatencyMillis, cancellationToken);
        }

        private async Task<(DateTime, List<Team>, RunnerProperties)> RefreshConfiguration(CancellationToken cancellationToken)
        {
            Log.Information("Refreshing Configuration...");
            await using (var lifetimeScope = _lifetimeScope.BeginLifetimeScope())
            {
                var dbContext = lifetimeScope.Resolve<AdventureDayBackendDbContext>();
                var properties = await dbContext.RunnerProperties.FirstOrDefaultAsync(_ =>
                    _.Name == RunnerProperties.DefaultRunnerPropertiesName, cancellationToken);
                var teams = await dbContext.Teams.ToListAsync(cancellationToken);
                return (DateTime.UtcNow, teams, properties);
            }
        }
        
        private void InvokePlayerWithFireAndForget(
            PlayerType playerType,
            Team team,
            CancellationToken cancellationToken)
        {
            var httpTimeout = TimeSpan.FromSeconds(5);
            
            Task.Run(async () =>
            {
                var startTimestamp = DateTime.UtcNow;
                MatchReport report = null;
                try
                {
                    Log.Debug($"Team {team.Name} vs. Player {playerType.ToString()}");
                    var player = CreatePlayerFromType(playerType, team, httpTimeout);

                    report = await player.Play(cancellationToken);
                }
                catch (TaskCanceledException exception)
                {
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
                catch (MatchCanceledException ex)
                {
                    report = MatchReport.FromCancellation(ex.Message);
                }
                catch (Exception exception)
                {
                    // Should rarely occur.
                    Log.Error(exception,
                        $"Issue in Fire and Forget for Team {team.Name} URI: {team.GameEngineUri}");
                    report = MatchReport.FromError("HTTP Timeout");
                }

                await PersistStatistics(DateTime.UtcNow - startTimestamp, team, report, cancellationToken);

            }, cancellationToken).Forget();
        }

        private async Task PersistStatistics(TimeSpan responseTime, Team team, MatchReport report, CancellationToken cancellationToken)
        {
            try
            {
                await using (var lifetimeScope = _lifetimeScope.BeginLifetimeScope())
                {
                    var error = report.Status != MatchRating.Success ? 1 : 0;
                    var loss = report.HasLost ? 1 : 0;
                    var win = report.HasWon ? 1 : 0;

                    var dbContext = lifetimeScope.Resolve<AdventureDayBackendDbContext>();
                    int rowsAffected = await dbContext.TeamScores.Where(_ => _.TeamId == team.Id)
                        .BatchUpdateAsync(_ =>
                        new TeamScore()
                        {
                            Costs = _.Costs + report.Cost,
                            Income = _.Income + report.Income,
                            Errors = _.Errors + error,
                            Loses = _.Loses + loss,
                            Wins = _.Wins + win
                        }, cancellationToken);

                    if (rowsAffected != 1)
                    {
                        // This code can run into concurrency issues.
                        // That is at the beginning two or more threads might 
                        // detect a non-existing score entry. This results in an
                        // Update exception, that we will swallow.
                        Log.Warning("Found no score record for team. Creating first one.");
                        await dbContext.TeamScores.AddAsync(new TeamScore()
                        {
                            TeamId = team.Id,
                            Costs = report.Cost,
                            Income = report.Income,
                            Errors = error,
                            Loses = loss,
                            Wins = win
                        }, cancellationToken);
                        await dbContext.SaveChangesAsync(cancellationToken);
                    }

                    if (report.HasLogEntry)
                    {
                        await dbContext.TeamLogEntries.AddAsync(new TeamLogEntry()
                        {
                            TeamId = team.Id,
                            Reason = report.Reason,
                            Status = report.Status.ToString(),
                            Timestamp = DateTime.UtcNow,
                            ResponeTimeMs = responseTime.Milliseconds
                        }, cancellationToken);
                        await dbContext.SaveChangesAsync(cancellationToken);
                    }
                }
            }
            catch (Exception exception)
            {
                Log.Error(exception, $"Failed to log entries for Team {team.Name} (ID: {team.Id})");
            }
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
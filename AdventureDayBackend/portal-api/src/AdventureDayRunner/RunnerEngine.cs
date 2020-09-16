using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AdventureDayRunner.Model;
using AdventureDayRunner.Players;
using AdventureDayRunner.Players.PseudoPlayers;
using AdventureDayRunner.Players.RealPlayers;
using AdventureDayRunner.Utils;
using Autofac;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Prometheus;
using Serilog;
using team_management_api.Data;
using team_management_api.Data.Runner;

namespace AdventureDayRunner
{
    public class RunnerEngine
    {
        private static readonly Counter PlayerInvocationsMetric = Metrics
            .CreateCounter("player_invocations", "Number of player invocations.");
        
        private readonly int _refreshTimeoutInSeconds = 5;
        private readonly ILifetimeScope _lifetimeScope;
        private readonly IConfiguration _configuration;

        public RunnerEngine(ILifetimeScope lifetimeScope, IConfiguration configuration)
        {
            _lifetimeScope = lifetimeScope;
            _configuration = configuration;
        }

        private async Task CreateEmptyScoring(CancellationToken cancellationToken)
        {
            Log.Information("Try to set initial team score.");
            try
            {
                // TODO: This is a bit lazy ass and relies on the PRIM KEY Constraint.
                var (lastRefresh, teams, properties) = await RefreshConfiguration(cancellationToken);
                await using (var lifetimeScope = _lifetimeScope.BeginLifetimeScope())
                {
                    var dbContext = lifetimeScope.Resolve<AdventureDayBackendDbContext>();
                    foreach (var team in teams)
                    {
                        Log.Warning("Found no score record for team. Creating first one.");
                        await dbContext.TeamScores.AddAsync(new TeamScore()
                        {
                            TeamId = team.Id,
                            Costs = 0,
                            Income = 0,
                            Errors = 0,
                            Loses = 0,
                            Wins = 0
                        }, cancellationToken);
                    }

                    await dbContext.SaveChangesAsync(cancellationToken);
                    Log.Information("Initial team score set.");
                }
            }
            catch (Exception exception)
            {
                Log.Error(exception, "Could not configure initial team score. Scoring Table might already be initialized.");
            }
        }

        public async Task Run(CancellationToken cancellationToken)
        {
            await CreateEmptyScoring(cancellationToken);
            
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
                        FireAndForgetForAllTeamsAndPlayers(phaseConfiguration, teams, cancellationToken);
                        await Task.Delay(phaseConfiguration.RequestExecutorLatencyMillis, cancellationToken);
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

        private void FireAndForgetForAllTeamsAndPlayers(
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
            PlayerInvocationsMetric.Inc();
            
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
                    var errorId = Guid.NewGuid();
                    
                    // If our cancellation token was not set, the
                    // HTTP timeout has triggered.
                    if (!cancellationToken.IsCancellationRequested)
                    {
                        Log.Error($"{errorId} HTTP Timeout.");
                        report = MatchReport.FromError($"Smoorghs are unable to play - no answer within {httpTimeout.Seconds} seconds (HTTP Timeout)");
                    }
                    else
                    {
                        report = MatchReport.FromError($"General error. Reference: {errorId}");
                        Log.Error(exception: exception, $"{errorId} TaskCanceledException | No HTTP Timeout detected.");
                    }
                }
                catch (MatchCanceledException ex)
                {
                    report = MatchReport.FromCancellation(ex.Message);
                }
                catch (Exception exception)
                {
                    if (exception.Message.Contains("An invalid request URI was provided."))
                    {
                        Log.Error($"No backend URI for team {team.Name} (ID: {team.Id}) found.");
                        report = MatchReport.FromError($"Smoorghs are unable to play - your backend URI is not configured");
                    }
                    else
                    {
                        var errorId = Guid.NewGuid();
                        Log.Error(exception, $"{errorId} Team {team.Name} (ID: {team.Id})");
                        report = MatchReport.FromError($"General error. Reference: {errorId}");
                    }
                }

                await PersistStatistics(DateTime.UtcNow - startTimestamp, team, report, cancellationToken);

            }, cancellationToken).Forget();
        }

        private async Task PersistStatistics(TimeSpan responseTime, Team team, MatchReport report, CancellationToken cancellationToken)
        {
            if (team == null) { throw new ArgumentNullException(nameof(team)); }
            if (report == null) { throw new ArgumentNullException(nameof(report)); }
            if (cancellationToken == null) { throw new ArgumentNullException(nameof(cancellationToken)); }
            
            try
            {
                await using (var lifetimeScope = _lifetimeScope.BeginLifetimeScope())
                {
                    var dbContext = lifetimeScope.Resolve<AdventureDayBackendDbContext>();
                    
                    // Errors
                    var error = report.HasError ? 1 : 0;
                    var loss = report.HasLost ? 1 : 0;
                    var win = report.HasWon ? 1 : 0;

                    if (report.MatchRatingStatus != MatchRating.Ignore)
                    {
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
                    }

                    if (report.HasLogEntry)
                    {
                        await dbContext.TeamLogEntries.AddAsync(new TeamLogEntry()
                        {
                            TeamId = team.Id,
                            Reason = report.Reason,
                            Status = report.LogEntryStatus,
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
        
        private IPlayer CreatePlayerFromType(PlayerType playerType, Team team, TimeSpan httpTimeout)
        {
            IPlayer player = playerType switch
            {
                PlayerType.Pattern => new PatternPlayer(_configuration, team, httpTimeout),
                PlayerType.Random => new RandomPlayer(_configuration, team, httpTimeout),
                PlayerType.Fixed => new FixedPlayer(_configuration, team, httpTimeout),
                PlayerType.Iterative => new IterativePlayer(_configuration, team, httpTimeout),
                PlayerType.Bet => new BetPlayer(_configuration, team, httpTimeout),
                PlayerType.CostCalculator => new CostCalculatorPlayer(_configuration, team, httpTimeout),
                PlayerType.SecurityHack => new SecurityHackPlayer(_configuration, team, httpTimeout),
                PlayerType.Mass => new MassPlayer(_configuration, team, httpTimeout),
                _ => throw new InvalidOperationException("Unexpected enum value.")
            };
            
            return player;
        }
    }
}
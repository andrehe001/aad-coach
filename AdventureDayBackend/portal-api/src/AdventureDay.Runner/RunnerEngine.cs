using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AdventureDay.DataModel;
using AdventureDay.DataModel.Runner;
using AdventureDay.Runner.Players;
using AdventureDay.Runner.Players.PseudoPlayers;
using AdventureDay.Runner.Players.RealPlayers;
using AdventureDay.Runner.Utils;
using Autofac;
using DeepEqual.Syntax;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Prometheus;
using Serilog;

namespace AdventureDay.Runner
{
    public class RunnerEngine
    {
        private static readonly Counter PlayerInvocationsMetric = Metrics
            .CreateCounter("player_invocations", "Number of player invocations.");
        
        private readonly int _refreshTimeoutInSeconds = 5;
        private readonly ILifetimeScope _lifetimeScope;
        private readonly IConfiguration _configuration;

        //private RunnerProperties _runnerProperties = RunnerProperties.CreateDefault();
        //private List<Team> _teams = new List<Team>();

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
                var (_, _, teams) = await RefreshConfiguration(cancellationToken);
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

            CancellationTokenSource playerCancellationTokenSource = null;
            while (!cancellationToken.IsCancellationRequested) // until we cancel completely externally
            {
                var (lastRefresh, runnerProperties, teams) = await RefreshConfiguration(cancellationToken);
                var currentPhaseConfiguration = runnerProperties.PhaseConfigurations[runnerProperties.CurrentPhase];
                var currentPlayerTypes = currentPhaseConfiguration.PlayerTypes.ToList();
                
                Log.Information($"Runner Status: {runnerProperties.RunnerStatus.ToString()}");
                switch (runnerProperties.RunnerStatus)
                {
                    case RunnerStatus.Started:
                        if (playerCancellationTokenSource == null)
                        {
                            playerCancellationTokenSource = new CancellationTokenSource();
                            FireAndForgetForAllTeamsAndPlayers(runnerProperties.CurrentPhase, teams, currentPlayerTypes,
                                playerCancellationTokenSource.Token);
                        }
                        break;
                    case RunnerStatus.Stopped:
                        playerCancellationTokenSource?.Cancel();
                        playerCancellationTokenSource = null;
                        break;
                    default:
                        throw new InvalidOperationException($"Unexpected RunnerStatus: {runnerProperties.RunnerStatus.ToString()}");
                }

                // W8 for 5secs, before updating from config.
                await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
                var (_, newRunnerProperties, newTeams) = await RefreshConfiguration(cancellationToken);
                if (!newRunnerProperties.IsDeepEqual(runnerProperties) || !newTeams.IsDeepEqual(teams))
                {
                    Log.Information("Config change detected, re-configure all player threads.");
                    playerCancellationTokenSource?.Cancel();
                    playerCancellationTokenSource = null;
                }
            }
        }

        private void FireAndForgetForAllTeamsAndPlayers(
            RunnerPhase currentPhase, List<Team> teams, 
            List<KeyValuePair<PlayerType, int>> playerTypes, 
            CancellationToken cancellationToken)
        {
            foreach (var team in teams)
            {
                foreach (var playerType in playerTypes)
                {
                    Task.Run(async () =>
                    {
                        while (!cancellationToken.IsCancellationRequested)
                        {
                            InvokePlayerWithFireAndForget(playerType.Key, team, playerType.Value, currentPhase, cancellationToken);
                            await Task.Delay(playerType.Value, cancellationToken);
                        }
                    }, cancellationToken).Forget();
                }
            }
        }

        private async Task<(DateTime, RunnerProperties, List<Team>)> RefreshConfiguration(CancellationToken cancellationToken)
        {
            await using (var lifetimeScope = _lifetimeScope.BeginLifetimeScope())
            {
                var dbContext = lifetimeScope.Resolve<AdventureDayBackendDbContext>();
                var runnerProperties = await dbContext.RunnerProperties.FirstOrDefaultAsync(_ =>
                    _.Name == RunnerProperties.DefaultRunnerPropertiesName, cancellationToken);
                var teams = await dbContext.Teams.ToListAsync(cancellationToken);
                
                return (DateTime.UtcNow, runnerProperties, teams);
            }
        }
        
        private void InvokePlayerWithFireAndForget(
            PlayerType playerType,
            Team team,
            int delay,
            RunnerPhase phase,
            CancellationToken cancellationToken)
        {
            PlayerInvocationsMetric.Inc();
            
            var httpTimeout = TimeSpan.FromSeconds(5);
            
            Task.Run(async () =>
            {
                if (team == null)
                {
                    Log.Warning($"Team configuration not found TeamId: {team.Id}.");
                    return;
                }
                
                var startTimestamp = DateTime.UtcNow;
                MatchReport report = null;
                try
                {                    
                    var player = CreatePlayerFromType(playerType, team, httpTimeout);
                    Log.Information($"Team {team.Name} vs. {player.Name} (Latency: {delay} Phase: {phase})");

                    report = await player.Play(cancellationToken);
                }
                catch (TaskCanceledException exception)
                {
                    var errorId = Guid.NewGuid();
                    
                    // If our cancellation token was not set, the
                    // HTTP timeout has triggered.
                    if (!cancellationToken.IsCancellationRequested)
                    {
                        Log.Information($"{errorId} HTTP Timeout | {team.Name} (ID: {team.Id}).");
                        report = MatchReport.FromError($"Smoorghs are unable to play - no answer within {httpTimeout.Seconds} seconds (HTTP Timeout)");
                    }
                    else
                    {
                        Log.Error(exception: exception, $"{errorId} Team {team.Name} (ID: {team.Id}) TaskCanceledException | No HTTP Timeout detected.");
                        report = MatchReport.FromError($"General error. Reference: {errorId}");
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
                        if (phase > RunnerPhase.Phase1_Deployment)
                        {
                            Log.Debug($"No backend URI for team {team.Name} (ID: {team.Id}) found.");
                            report = MatchReport.FromError(
                                $"Smoorghs are unable to play - your backend URI is not configured");
                        }
                        else
                        {
                            Log.Debug($"No backend URI for team {team.Name} (ID: {team.Id}) found (IGNORED - {phase}).");
                            report = MatchReport.FromBackendUriMissing("Smoorghs are unable to play - your backend URI is not configured");
                        }
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
                            Log.Warning($"Found no score record for team {team.Name} ({team.Id}). Creating first one.");
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
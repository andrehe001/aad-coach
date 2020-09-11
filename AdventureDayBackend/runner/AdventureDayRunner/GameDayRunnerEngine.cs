using System;
using System.Threading;
using System.Threading.Tasks;
using AdventureDayRunner.Players;
using AdventureDayRunner.Shared;
using Serilog;

namespace AdventureDayRunner
{
    public class GameDayRunnerEngine
    {
        public async Task Run(CancellationToken cancellationToken)
        {
            var adventureDayRunnerProperties = await Utils.ReadPropertiesFromDb(cancellationToken);

            do
            {
                var currentPhase = adventureDayRunnerProperties.CurrentPhase;
                var phaseConfiguration = adventureDayRunnerProperties.PhaseConfigurations[currentPhase];
                
                if (adventureDayRunnerProperties.AdventureDayRunnerStatus == AdventureDayRunnerStatus.Running)
                {
                    Log.Information(
                        $"Phase: {currentPhase.ToString()} Latency: {phaseConfiguration.RequestExecutorLatencyMillis}");
                    
                    // Fire forget match requests for all configured players.
                    foreach (var team in adventureDayRunnerProperties.Teams)
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
                    Log.Information($"Status: {adventureDayRunnerProperties.AdventureDayRunnerStatus.ToString()} (retry in 5secs)");
                    await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
                }
                
                adventureDayRunnerProperties = await Utils.ReadPropertiesFromDb(cancellationToken);
            } while (!cancellationToken.IsCancellationRequested);
        }

        private static void InvokePlayerWithFireAndForget(
            PlayerType playerType,
            AdventureDayTeamInformation teamInformation,
            CancellationToken cancellationToken)
        {
            // TODO configurable.
            var httpTimeout = TimeSpan.FromSeconds(5);
            Task.Run(async () =>
            {
                try
                {
                    PlayerBase player;
                    switch (playerType)
                    {
                        case PlayerType.Pattern:
                            player = new PatternPlayer(teamInformation, httpTimeout);
                            break;
                        case PlayerType.Random:
                            player = new RandomPlayer(teamInformation, httpTimeout);
                            break;
                        
                        default:
                            throw new InvalidOperationException("Unexpected enum value.");
                    }

                    var matchResponse = await player.Play(cancellationToken);

                    Log.Information(matchResponse == null
                        ? $"Team {teamInformation.Name}: Failed."
                        : $"Team {teamInformation.Name}: {matchResponse.MatchOutcome.ToString()}");

                    // TODO matchResponse = null --> Failure.
                    //      matchResponse -> statistics based on phase.
                }
                catch (Exception exception)
                {
                    Log.Error(exception,
                        $"Issue in Fire and Forget for Team {teamInformation.Name} URI: {teamInformation.GameEngineUri}");
                }
            }, cancellationToken).Forget();
        }
    }
}
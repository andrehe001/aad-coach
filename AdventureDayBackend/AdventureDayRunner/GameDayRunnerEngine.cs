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
                        $"Phase: {currentPhase.ToString()} NumberOfReqPerTeam: {phaseConfiguration.NumberOfRequestExecutorsPerTeam} Latency: {phaseConfiguration.RequestExecutorLatencyMillis}");
                    foreach (var team in adventureDayRunnerProperties.Teams)
                    {
                        for (int i = 0; i < phaseConfiguration.NumberOfRequestExecutorsPerTeam; i++)
                        {
                            InvokePlayerWithFireAndForget(team, cancellationToken);
                        }
                    }
                    
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

        private static void InvokePlayerWithFireAndForget(AdventureDayTeamInformation teamInformation,
            CancellationToken cancellationToken)
        {
            Task.Run(async () =>
            {
                try
                {
                    // TODO: Record match stats.
                    var matchStatistics = await new RandomPlayer(teamInformation.GameEngineUri)
                        .Play(cancellationToken);
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
namespace team_management_api.Data.Runner
{
    public static class DefaultRunnerPhasesConfiguration
    {
        public static readonly RunnerPhasesConfiguration DefaultConfiguration = new RunnerPhasesConfiguration()
        {
            {
                RunnerPhase.Phase1_Deployment, 
                new RunnerPhaseConfigurationItem()
                {
                    RequestExecutorLatencyMillis = 1000,
                    PlayerTypes = new [] { PlayerType.Random }
                }
            },
            {
                RunnerPhase.Phase2_Change, 
                new RunnerPhaseConfigurationItem()
                {
                    RequestExecutorLatencyMillis = 1000,
                    PlayerTypes = new [] { PlayerType.Random }
                }
            },
            {
                RunnerPhase.Phase3_Monitoring, 
                new RunnerPhaseConfigurationItem()
                {
                    RequestExecutorLatencyMillis = 1000,
                    PlayerTypes = new [] { PlayerType.Random }
                }
            },
            {
                RunnerPhase.Phase4_Scale, 
                new RunnerPhaseConfigurationItem()
                {
                    RequestExecutorLatencyMillis = 100,
                    PlayerTypes = new [] { PlayerType.Random }
                }
            },
            {
                RunnerPhase.Phase5_Security, 
                new RunnerPhaseConfigurationItem()
                {
                    RequestExecutorLatencyMillis = 500,
                    PlayerTypes = new [] { PlayerType.Random }
                }
            },
            {
                RunnerPhase.Phase6_Intelligence, 
                new RunnerPhaseConfigurationItem()
                {
                    RequestExecutorLatencyMillis = 500,
                    PlayerTypes = new [] { PlayerType.Pattern }
                }
            }
        };
    }
}
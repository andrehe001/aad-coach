namespace AdventureDay.ManagementApi.Data.Runner
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
                    PlayerTypes = new []
                    {
                        PlayerType.Random, 
                        PlayerType.Fixed
                    }
                }
            },
            {
                RunnerPhase.Phase2_Change, 
                new RunnerPhaseConfigurationItem()
                {
                    RequestExecutorLatencyMillis = 1000,
                    PlayerTypes = new []
                    {
                        PlayerType.Random, 
                        PlayerType.Fixed, 
                        PlayerType.Bet
                    }
                }
            },
            {
                RunnerPhase.Phase3_Monitoring, 
                new RunnerPhaseConfigurationItem()
                {
                    RequestExecutorLatencyMillis = 1000,
                    PlayerTypes = new []
                    {
                        PlayerType.Random, 
                        PlayerType.Fixed, 
                        PlayerType.Bet, 
                        PlayerType.CostCalculator
                    }
                }
            },
            {
                RunnerPhase.Phase4_Scale, 
                new RunnerPhaseConfigurationItem()
                {
                    RequestExecutorLatencyMillis = 100,
                    PlayerTypes = new []
                    {
                        PlayerType.Random, 
                        PlayerType.Fixed, 
                        PlayerType.Bet, 
                        // PlayerType.CostCalculator,  # TODO: Cost calculation API does not like "hammer mode"
                        PlayerType.Mass
                    }
                }
            },
            {
                RunnerPhase.Phase5_Security, 
                new RunnerPhaseConfigurationItem()
                {
                    RequestExecutorLatencyMillis = 500,
                    PlayerTypes = new []
                    {
                        PlayerType.Random,
                        PlayerType.Fixed, 
                        PlayerType.Bet, 
                        PlayerType.CostCalculator,
                        PlayerType.SecurityHack
                    }
                }
            },
            {
                RunnerPhase.Phase6_Intelligence, 
                new RunnerPhaseConfigurationItem()
                {
                    RequestExecutorLatencyMillis = 500,
                    PlayerTypes = new [] {
                        PlayerType.Random,
                        PlayerType.Fixed,
                        PlayerType.Bet,
                        PlayerType.CostCalculator,
                        PlayerType.SecurityHack,
                        PlayerType.Iterative,
                        PlayerType.Pattern
                    }
                }
            }
        };
    }
}
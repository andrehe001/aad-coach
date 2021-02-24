using System.Collections.Generic;

namespace AdventureDay.DataModel.Runner
{
    public static class DefaultRunnerPhasesConfiguration
    {
        public static readonly RunnerPhasesConfiguration DefaultConfiguration = new RunnerPhasesConfiguration()
        {
            {
                RunnerPhase.Phase1_Deployment,
                new RunnerPhaseConfigurationItem()
                {
                    PlayerTypes = new Dictionary<PlayerType, int>()
                    {
                        { PlayerType.Random, 1000},
                        { PlayerType.Fixed, 1000},
                    }
                }
            },
            {
                RunnerPhase.Phase2_Change,
                new RunnerPhaseConfigurationItem()
                {
                    PlayerTypes = new Dictionary<PlayerType, int>()
                    {
                        { PlayerType.Random, 1000},
                        { PlayerType.Fixed, 1000},
                        { PlayerType.Bet, 1000},
                    }
                }
            },
            {
                RunnerPhase.Phase3_Monitoring,
                new RunnerPhaseConfigurationItem()
                {
                    PlayerTypes = new Dictionary<PlayerType, int>()
                    {
                        { PlayerType.Random, 1000},
                        { PlayerType.Fixed, 1000},
                        { PlayerType.Bet, 1000},
                        { PlayerType.CostCalculator, 60 * 1000},
                    }
                }
            },
            {
                RunnerPhase.Phase4_Scale,
                new RunnerPhaseConfigurationItem()
                {
                    PlayerTypes = new Dictionary<PlayerType, int>()
                    {

                        { PlayerType.Random, 100},
                        { PlayerType.Fixed, 100},
                        { PlayerType.Bet, 100},
                        { PlayerType.CostCalculator, 60 * 1000},
                        { PlayerType.Mass, 500},
                    }
                }
            },
            {
                RunnerPhase.Phase5_Security,
                new RunnerPhaseConfigurationItem()
                {
                    PlayerTypes = new Dictionary<PlayerType, int>()
                    {
                        { PlayerType.Random, 1000},
                        { PlayerType.Fixed, 1000},
                        { PlayerType.Bet, 1000},
                        { PlayerType.CostCalculator, 60 * 1000},
                        { PlayerType.SecurityHack, 1000},
                    }
                }
            },
            {
                RunnerPhase.Phase6_Intelligence,
                new RunnerPhaseConfigurationItem()
                {
                    PlayerTypes = new Dictionary<PlayerType, int>()
                    {
                        { PlayerType.Random, 1000},
                        { PlayerType.Fixed, 1000},
                        { PlayerType.Bet, 1000},
                        { PlayerType.CostCalculator, 60 * 1000},
                        { PlayerType.SecurityHack, 1000},
                        { PlayerType.Iterative, 1000},
                        { PlayerType.Pattern, 1000},
                        { PlayerType.EasyPeasy, 1000},
                    }
                }
            }
        };
    }
}
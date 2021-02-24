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
                        { PlayerType.Random, 4000},
                        { PlayerType.Fixed, 4000},
                    }
                }
            },
            {
                RunnerPhase.Phase2_Change,
                new RunnerPhaseConfigurationItem()
                {
                    PlayerTypes = new Dictionary<PlayerType, int>()
                    {
                        { PlayerType.Random, 4000},
                        { PlayerType.Fixed, 4000},
                        { PlayerType.Bet, 4000},
                    }
                }
            },
            {
                RunnerPhase.Phase3_Monitoring,
                new RunnerPhaseConfigurationItem()
                {
                    PlayerTypes = new Dictionary<PlayerType, int>()
                    {
                        { PlayerType.Random, 4000},
                        { PlayerType.Fixed, 4000},
                        { PlayerType.Bet, 4000},
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

                        { PlayerType.Random, 400},
                        { PlayerType.Fixed, 400},
                        { PlayerType.Bet, 400},
                        { PlayerType.CostCalculator, 60 * 1000},
                        { PlayerType.Mass, 400},
                    }
                }
            },
            {
                RunnerPhase.Phase5_Security,
                new RunnerPhaseConfigurationItem()
                {
                    PlayerTypes = new Dictionary<PlayerType, int>()
                    {
                        { PlayerType.Random, 4000},
                        { PlayerType.Fixed, 4000},
                        { PlayerType.Bet, 4000},
                        { PlayerType.CostCalculator, 60 * 1000},
                        { PlayerType.SecurityHack, 4000},
                    }
                }
            },
            {
                RunnerPhase.Phase6_Intelligence,
                new RunnerPhaseConfigurationItem()
                {
                    PlayerTypes = new Dictionary<PlayerType, int>()
                    {
                        { PlayerType.Random, 4000},
                        { PlayerType.Fixed, 4000},
                        { PlayerType.Bet, 4000},
                        { PlayerType.CostCalculator, 60 * 1000},
                        { PlayerType.SecurityHack, 4000},
                        { PlayerType.Iterative, 4000},
                        { PlayerType.Pattern, 4000},
                        { PlayerType.EasyPeasy, 4000},
                    }
                }
            }
        };
    }
}
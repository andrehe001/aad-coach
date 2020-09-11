using System.Collections.Generic;

namespace AdventureDayRunner.Shared
{
    public static class DefaultAdventureDayPhaseConfiguration
    {
        public static readonly Dictionary<AdventureDayPhase, AdventureDayPhaseConfiguration> DefaultConfiguration = new Dictionary<AdventureDayPhase, AdventureDayPhaseConfiguration>()
        {
            {
                AdventureDayPhase.Phase1_Deployment, 
                new AdventureDayPhaseConfiguration()
                {
                    RequestExecutorLatencyMillis = 1000,
                    PlayerTypes = new [] { PlayerType.Random }
                }
            },
            {
                AdventureDayPhase.Phase2_Change, 
                new AdventureDayPhaseConfiguration()
                {
                    RequestExecutorLatencyMillis = 1000,
                    PlayerTypes = new [] { PlayerType.Random }
                }
            },
            {
                AdventureDayPhase.Phase3_Monitoring, 
                new AdventureDayPhaseConfiguration()
                {
                    RequestExecutorLatencyMillis = 1000,
                    PlayerTypes = new [] { PlayerType.Random }
                }
            },
            {
                AdventureDayPhase.Phase4_Scale, 
                new AdventureDayPhaseConfiguration()
                {
                    RequestExecutorLatencyMillis = 100,
                    PlayerTypes = new [] { PlayerType.Random }
                }
            },
            {
                AdventureDayPhase.Phase5_Security, 
                new AdventureDayPhaseConfiguration()
                {
                    RequestExecutorLatencyMillis = 500,
                    PlayerTypes = new [] { PlayerType.Random }
                }
            },
            {
                AdventureDayPhase.Phase6_Intelligence, 
                new AdventureDayPhaseConfiguration()
                {
                    RequestExecutorLatencyMillis = 500,
                    PlayerTypes = new [] { PlayerType.Pattern }
                }
            }
        };
    }
}
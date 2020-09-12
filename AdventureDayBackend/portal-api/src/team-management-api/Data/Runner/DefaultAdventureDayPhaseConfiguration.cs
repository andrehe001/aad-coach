using System.Collections.Generic;

namespace team_management_api.Data.Runner
{
    public static class DefaultAdventureDayPhaseConfiguration
    {
        public static readonly AdventureDayPhaseConfiguration DefaultConfiguration = new AdventureDayPhaseConfiguration()
        {
            {
                AdventureDayPhase.Phase1_Deployment, 
                new AdventureDayPhaseConfigurationItem()
                {
                    RequestExecutorLatencyMillis = 1000,
                    PlayerTypes = new [] { PlayerType.Random }
                }
            },
            {
                AdventureDayPhase.Phase2_Change, 
                new AdventureDayPhaseConfigurationItem()
                {
                    RequestExecutorLatencyMillis = 1000,
                    PlayerTypes = new [] { PlayerType.Random }
                }
            },
            {
                AdventureDayPhase.Phase3_Monitoring, 
                new AdventureDayPhaseConfigurationItem()
                {
                    RequestExecutorLatencyMillis = 1000,
                    PlayerTypes = new [] { PlayerType.Random }
                }
            },
            {
                AdventureDayPhase.Phase4_Scale, 
                new AdventureDayPhaseConfigurationItem()
                {
                    RequestExecutorLatencyMillis = 100,
                    PlayerTypes = new [] { PlayerType.Random }
                }
            },
            {
                AdventureDayPhase.Phase5_Security, 
                new AdventureDayPhaseConfigurationItem()
                {
                    RequestExecutorLatencyMillis = 500,
                    PlayerTypes = new [] { PlayerType.Random }
                }
            },
            {
                AdventureDayPhase.Phase6_Intelligence, 
                new AdventureDayPhaseConfigurationItem()
                {
                    RequestExecutorLatencyMillis = 500,
                    PlayerTypes = new [] { PlayerType.Pattern }
                }
            }
        };
    }
}
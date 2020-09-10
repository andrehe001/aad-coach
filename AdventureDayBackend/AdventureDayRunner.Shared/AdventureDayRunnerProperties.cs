using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AdventureDayRunner.Shared
{
    public class AdventureDayRunnerProperties
    {
        public static AdventureDayRunnerProperties CreateDefault(string name)
        {
            return new AdventureDayRunnerProperties()
            {
                Name = name,
                Teams = new AdventureDayTeamInformation[] { },
                AdventureDayRunnerStatus = AdventureDayRunnerStatus.Stopped,
                PhaseConfigurations = DefaultAdventureDayPhaseConfiguration.DefaultConfiguration,
                CurrentPhase = AdventureDayPhase.Phase1_Deployment
            };
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public AdventureDayRunnerStatus AdventureDayRunnerStatus { get; set; }

        public AdventureDayTeamInformation[] Teams { get; set; }
        
        public Dictionary<AdventureDayPhase, AdventureDayPhaseConfiguration> PhaseConfigurations { get; set; }
        
        public string Name { get; set; }
        
        public AdventureDayPhase CurrentPhase { get; set; }
    }
}
using System.Collections.Generic;
using Newtonsoft.Json;

namespace team_management_api.Data.Runner
{
    public class AdventureDayPhaseConfiguration : Dictionary<AdventureDayPhase, AdventureDayPhaseConfigurationItem>
    {
        public static AdventureDayPhaseConfiguration FromJson(string json)
        {
            return JsonConvert.DeserializeObject<AdventureDayPhaseConfiguration>(json);
        }
    }

    public static class AdventureDayPhaseConfigurationExtensions
    {
        public static string ToJson(this AdventureDayPhaseConfiguration configuration)
        {
            return JsonConvert.SerializeObject(configuration, Formatting.Indented);
        }
    }
}
using System.Collections.Generic;
using Newtonsoft.Json;

namespace team_management_api.Data.Runner
{
    public class RunnerPhasesConfiguration : Dictionary<RunnerPhase, RunnerPhaseConfigurationItem>
    {
        public static RunnerPhasesConfiguration FromJson(string json)
        {
            return JsonConvert.DeserializeObject<RunnerPhasesConfiguration>(json);
        }
    }

    public static class AdventureDayPhaseConfigurationExtensions
    {
        public static string ToJson(this RunnerPhasesConfiguration configuration)
        {
            return JsonConvert.SerializeObject(configuration, Formatting.Indented);
        }
    }
}
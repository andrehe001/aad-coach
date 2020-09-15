using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace team_management_api.Data.Runner
{
    public class RunnerPhasesConfiguration : Dictionary<RunnerPhase, RunnerPhaseConfigurationItem>
    {
        private static readonly StringEnumConverter StringEnumConverter = new StringEnumConverter();
        public static RunnerPhasesConfiguration FromJson(string json)
        {
            return JsonConvert.DeserializeObject<RunnerPhasesConfiguration>(json, StringEnumConverter);
        }
    }

    public static class AdventureDayPhaseConfigurationExtensions
    {
        private static readonly StringEnumConverter StringEnumConverter = new StringEnumConverter();
        public static string ToJson(this RunnerPhasesConfiguration configuration)
        {
            return JsonConvert.SerializeObject(configuration, Formatting.Indented, StringEnumConverter);
        }
    }
}
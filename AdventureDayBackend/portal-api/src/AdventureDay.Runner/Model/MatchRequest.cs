using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace AdventureDay.Runner.Model
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public enum Move
    {
        Rock = 0,
        Paper,
        Scissors,
        Metal,
        Snap
    }
}
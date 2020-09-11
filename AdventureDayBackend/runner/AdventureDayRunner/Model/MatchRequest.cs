using System.Text.Json.Serialization;

namespace AdventureDayRunner.Model
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Move
    {
        Rock = 0,
        Paper,
        Scissors,
        Metal,
        Snap
    }
}
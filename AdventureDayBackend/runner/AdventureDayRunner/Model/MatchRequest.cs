using System.Text.Json.Serialization;

namespace AdventureDayRunner.Model
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Move
    {
        Rock,
        Paper,
        Scissors,
        Metal,
        Snap
    }
}
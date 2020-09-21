using System;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AdventureDay.DataModel
{
    public class TeamLogEntry
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int TeamId { get; set; }

        public DateTime Timestamp { get; set; }

        public Team Team { get; set; }

        public int ResponeTimeMs { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public LogEntryStatus Status { get; set; }

        public string Reason { get; set; }
    }
}

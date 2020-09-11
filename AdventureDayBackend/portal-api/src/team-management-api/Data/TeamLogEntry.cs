using System;

namespace team_management_data
{
    public class TeamLogEntry
    {
        public int Id { get; set; }

        public int TeamId { get; set; }

        public DateTime Timestamp { get; set; }

        public Team Team { get; set; }

        public int ResponeTimeMs { get; set; }

        public string Status { get; set; }

        public string Reason { get; set; }
    }
}

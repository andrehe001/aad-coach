using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace team_management_data
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Member> Members { get; set; }

        public Guid SubscriptionId { get; set; }
        [JsonIgnore]
        public string TeamPassword { get; set; }
    }
}

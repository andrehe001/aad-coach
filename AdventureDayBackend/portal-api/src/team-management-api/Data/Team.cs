﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace team_management_api.Data
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Member> Members { get; set; }
        
        public string GameEngineUri { get; set; }

        public Guid SubscriptionId { get; set; }
        [JsonIgnore]
        public string TeamPassword { get; set; }
    }
}
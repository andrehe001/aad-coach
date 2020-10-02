using System;
using System.Collections.Generic;

namespace AdventureDay.DataModel
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Member> Members { get; set; }
        
        public string GameEngineUri { get; set; }
        public Guid TenantId { get; set; }

        public Guid SubscriptionId { get; set; }
        public string TeamPassword { get; set; }
    }
}

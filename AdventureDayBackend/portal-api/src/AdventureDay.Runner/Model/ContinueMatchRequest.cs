using System;

namespace AdventureDay.Runner.Model
{
    public class ContinueMatchRequest
    {
        public Guid MatchId { get; set; }
        
        public Move Move { get; set; }
    }
}
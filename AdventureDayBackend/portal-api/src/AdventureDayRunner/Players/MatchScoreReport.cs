using System.Collections.Generic;
using AdventureDayRunner.Model;

namespace AdventureDayRunner.Players
{
    public class MatchScoreReport
    {
        public static MatchScoreReport FromMatchResponse(MatchResponse matchResponse)
        {
            // TODO: Logic.
            return new MatchScoreReport();
        }

        public bool HasWon => true;

        public bool HasLost => !HasWon;

        public int Income => 10;

        public int Cost => 0;
        
        public List<string> Errors { get; set; }
        
    }
}
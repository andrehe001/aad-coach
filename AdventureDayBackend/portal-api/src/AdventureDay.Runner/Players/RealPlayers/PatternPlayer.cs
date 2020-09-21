using System;
using AdventureDay.DataModel;
using AdventureDay.Runner.Model;
using Microsoft.Extensions.Configuration;

namespace AdventureDay.Runner.Players.RealPlayers
{
    public class PatternPlayer : RealPlayerBase
    {
        public override string Name => "Libby";

        public PatternPlayer(IConfiguration configuration, Team team, TimeSpan httpTimeout) : base(configuration, team, httpTimeout)
        {

        }

        private Move GetPatternMove(int seq)
        {
            var values = Enum.GetValues(typeof(Move));
            return (Move)values.GetValue(seq % values.Length);
        }

        protected override Move GetFirstMove()
        {
            return GetPatternMove(0);
        }

        protected override Move GetNextMove(MatchResponse lastMatchResponse)
        {
            return GetPatternMove(lastMatchResponse.Turn);
        }
    }
}
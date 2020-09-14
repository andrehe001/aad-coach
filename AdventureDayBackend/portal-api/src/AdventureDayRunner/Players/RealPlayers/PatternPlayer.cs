using System;
using AdventureDayRunner.Model;
using team_management_api.Data;

namespace AdventureDayRunner.Players.RealPlayers
{
    public class PatternPlayer : RealPlayerBase
    {
        public override string Name => "Libby";

        public PatternPlayer(Team team, TimeSpan httpTimeout)
            : base(team, httpTimeout)
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
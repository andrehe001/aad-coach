using System;
using AdventureDayRunner.Model;
using team_management_api.Data;

namespace AdventureDayRunner.Players
{
    public class IterativePlayer : PlayerBase
    {
        private int _seq;

        protected override string Name => "Courtney";

        public IterativePlayer(Team team, TimeSpan httpTimeout)
            : base(team, httpTimeout)
        {
            _seq = 0;
        }

        private Move GetNextTickMove()
        {
            var values = Enum.GetValues(typeof(Move));
            return (Move)values.GetValue(_seq++ % values.Length);
        }

        protected override Move GetFirstMove()
        {
            return GetNextTickMove();
        }

        protected override Move GetNextMove(MatchResponse lastMatchResponse)
        {
            return GetNextTickMove();
        }
    }
}
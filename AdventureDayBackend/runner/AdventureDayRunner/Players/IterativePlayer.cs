using System;
using AdventureDayRunner.Model;
using AdventureDayRunner.Shared;

namespace AdventureDayRunner.Players
{
    public class IterativePlayer : PlayerBase
    {
        private int _seq;

        protected override string Name => "Courtney";

        public IterativePlayer(AdventureDayTeamInformation teamInformation, TimeSpan httpTimeout)
            : base(teamInformation, httpTimeout)
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
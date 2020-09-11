using System;
using AdventureDayRunner.Model;
using AdventureDayRunner.Shared;

namespace AdventureDayRunner.Players
{
    public class PatternPlayer : PlayerBase
    {
        public PatternPlayer(AdventureDayTeamInformation teamInformation, TimeSpan httpTimeout) : base(teamInformation, httpTimeout)
        {
        }

        protected override Move GetNextMove(int seq)
        {
            return Move.Metal;
        }
    }
}
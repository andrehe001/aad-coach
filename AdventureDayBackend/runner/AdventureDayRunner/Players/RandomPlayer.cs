using System;
using AdventureDayRunner.Model;
using AdventureDayRunner.Shared;
using Serilog;

namespace AdventureDayRunner.Players
{
    public class RandomPlayer : PlayerBase
    {
        public RandomPlayer(AdventureDayTeamInformation teamInformation, TimeSpan httpTimeout) : base(teamInformation, httpTimeout)
        {
        }

        private Move GetRandomMove()
        {            
            Array values = Enum.GetValues(typeof(Move));
            Random random = new Random();
            Move randomMove = (Move)values.GetValue(random.Next(values.Length));
            return randomMove;
        }

        protected override Move GetFirstMove()
        {
            return GetRandomMove();
        }

        protected override Move GetNextMove(MatchResponse lastMatchResponse)
        {
            return GetRandomMove();
        }
    }
}
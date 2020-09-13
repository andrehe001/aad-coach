using System;
using AdventureDayRunner.Model;
using AdventureDayRunner.Shared;
using Serilog;

namespace AdventureDayRunner.Players
{
    public class RandomPlayer : PlayerBase
    {
        private readonly Random _random;

        protected override string Name => "Lachlan";

        public RandomPlayer(AdventureDayTeamInformation teamInformation, TimeSpan httpTimeout) : base(teamInformation, httpTimeout)
        {
            _random = new Random();
        }

        private Move GetRandomMove()
        {            
            var values = Enum.GetValues(typeof(Move));
            return (Move)values.GetValue(_random.Next(values.Length));
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
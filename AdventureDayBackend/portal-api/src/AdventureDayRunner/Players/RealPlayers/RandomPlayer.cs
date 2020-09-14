using System;
using AdventureDayRunner.Model;
using Microsoft.Extensions.Configuration;
using team_management_api.Data;

namespace AdventureDayRunner.Players.RealPlayers
{
    public class RandomPlayer : RealPlayerBase
    {
        private readonly Random _random;

        public override string Name => "Lachlan";

        public RandomPlayer(IConfiguration configuration, Team team, TimeSpan httpTimeout) : base(configuration, team, httpTimeout)
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
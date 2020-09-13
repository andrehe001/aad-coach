using System;
using AdventureDayRunner.Model;
using team_management_api.Data;

namespace AdventureDayRunner.Players
{
    public class FixedPlayer : PlayerBase
    {
        private readonly Random _random;
        private Move _fixedMoved;

        protected override string Name => "Kye";

        public FixedPlayer(Team team, TimeSpan httpTimeout) : base(team, httpTimeout)
        {
            _random = new Random();
        }

        protected override Move GetFirstMove()
        {
            var values = Enum.GetValues(typeof(Move));
            _fixedMoved = (Move)values.GetValue(_random.Next(values.Length));
            return _fixedMoved;
        }

        protected override Move GetNextMove(MatchResponse lastMatchResponse)
        {
            return _fixedMoved;
        }
    }
}
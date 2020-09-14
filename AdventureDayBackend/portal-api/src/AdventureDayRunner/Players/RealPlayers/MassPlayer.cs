using System;
using AdventureDayRunner.Model;
using team_management_api.Data;

namespace AdventureDayRunner.Players.RealPlayers
{
    // Just acts the same as RandomPlayer 
    // - but triggers crazy CPU loops and SQL queries inside GameEngine
    public class MassPlayer : RealPlayerBase
    {
        private readonly Random _random;

        public override string Name => "Gloria";

        public MassPlayer(Team team, TimeSpan httpTimeout) : base(team, httpTimeout)
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
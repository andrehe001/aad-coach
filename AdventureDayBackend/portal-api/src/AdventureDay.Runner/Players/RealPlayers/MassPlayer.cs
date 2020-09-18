using System;
using AdventureDay.Runner.Model;
using Microsoft.Extensions.Configuration;
using team_management_api.Data;

namespace AdventureDay.Runner.Players.RealPlayers
{
    // Just acts the same as RandomPlayer 
    // - but triggers crazy CPU loops and SQL queries inside GameEngine
    public class MassPlayer : RealPlayerBase
    {
        private readonly Random _random;

        public override string Name => "Gloria";

        public MassPlayer(IConfiguration configuration, Team team, TimeSpan httpTimeout) : base(configuration, team, httpTimeout)
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
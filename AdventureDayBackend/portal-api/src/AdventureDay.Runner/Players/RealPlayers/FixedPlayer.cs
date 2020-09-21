using System;
using AdventureDay.DataModel;
using AdventureDay.Runner.Model;
using Microsoft.Extensions.Configuration;
using AdventureDay.ManagementApi.Data;

namespace AdventureDay.Runner.Players.RealPlayers
{
    public class FixedPlayer : RealPlayerBase
    {
        private readonly Random _random;
        private Move _fixedMoved;

        public override string Name => "Kye";

        public FixedPlayer(IConfiguration configuration, Team team, TimeSpan httpTimeout) : base(configuration, team, httpTimeout)
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
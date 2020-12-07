using System;
using AdventureDay.DataModel;
using AdventureDay.Runner.Model;
using Microsoft.Extensions.Configuration;

namespace AdventureDay.Runner.Players.RealPlayers
{
    public class FixedPlayer : RealPlayerBase
    {
        private static readonly Lazy<Move> _fixedMoved = new Lazy<Move>(() => GetRandomMove());

        private static Move GetRandomMove()
        {
            var random = new Random();
            var values = Enum.GetValues(typeof(Move));
            return (Move)values.GetValue(random.Next(values.Length));
        }

        public override string Name => "Kye";

        public FixedPlayer(IConfiguration configuration, Team team, TimeSpan httpTimeout) : base(configuration, team, httpTimeout)
        {
        }

        protected override Move GetFirstMove()
        {
            return _fixedMoved.Value;
        }

        protected override Move GetNextMove(MatchResponse lastMatchResponse)
        {
            return _fixedMoved.Value;
        }
    }
}
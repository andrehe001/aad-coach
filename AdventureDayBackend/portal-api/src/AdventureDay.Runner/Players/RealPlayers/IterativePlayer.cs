using System;
using AdventureDay.DataModel;
using AdventureDay.Runner.Model;
using Microsoft.Extensions.Configuration;

namespace AdventureDay.Runner.Players.RealPlayers
{
    public class IterativePlayer : RealPlayerBase
    {
        private int _seq;

        public override string Name => "Courtney";

        public IterativePlayer(IConfiguration configuration, Team team, TimeSpan httpTimeout) : base(configuration, team, httpTimeout)
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
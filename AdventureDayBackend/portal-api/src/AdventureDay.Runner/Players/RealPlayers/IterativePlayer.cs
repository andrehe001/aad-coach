using System;
using AdventureDay.DataModel;
using AdventureDay.Runner.Model;
using Microsoft.Extensions.Configuration;

namespace AdventureDay.Runner.Players.RealPlayers
{
    public class IterativePlayer : RealPlayerBase
    {
        private static int _runnerSeq = 0;

        private int _instanceSeq;

        public override string Name => "Courtney";

        public IterativePlayer(IConfiguration configuration, Team team, TimeSpan httpTimeout) : base(configuration, team, httpTimeout)
        {
            _instanceSeq = _runnerSeq++;
        }

        private Move GetNextTickMove()
        {
            var values = Enum.GetValues(typeof(Move));
            return (Move)values.GetValue(_instanceSeq++ % values.Length);
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
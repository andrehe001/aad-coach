using System;
using Microsoft.Extensions.Configuration;

using AdventureDay.DataModel;
using AdventureDay.Runner.Model;

namespace AdventureDay.Runner.Players.RealPlayers
{
    public class RandomPlayer : RealPlayerBase
    {
        public RandomPlayer(IConfiguration configuration, Team team, TimeSpan httpTimeout) : base(configuration, team, httpTimeout)
        {
        }

        public override string Name => "Lachlan";

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
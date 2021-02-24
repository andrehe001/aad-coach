using System;
using AdventureDay.DataModel;
using AdventureDay.Runner.Model;
using Microsoft.Extensions.Configuration;

namespace AdventureDay.Runner.Players.RealPlayers
{
    public class EasyPeasyPlayer : RealPlayerBase
    {
        public override string Name => "Kye";

        public EasyPeasyPlayer(IConfiguration configuration, Team team, TimeSpan httpTimeout) : base(configuration, team, httpTimeout)
        {
        }

        protected override Move GetFirstMove()
        {
            return Move.Rock;
        }

        protected override Move GetNextMove(MatchResponse lastMatchResponse)
        {
            return Move.Rock;
        }
    }
}
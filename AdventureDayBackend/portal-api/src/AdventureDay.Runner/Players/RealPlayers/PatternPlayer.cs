using System;
using AdventureDay.DataModel;
using AdventureDay.Runner.Model;
using Microsoft.Extensions.Configuration;

namespace AdventureDay.Runner.Players.RealPlayers
{
    public class PatternPlayer : RealPlayerBase
    {
        public override string Name => "Libby";

        public PatternPlayer(IConfiguration configuration, Team team, TimeSpan httpTimeout) : base(configuration, team, httpTimeout)
        {
        }

        protected override Move GetFirstMove()
        {
            return GetRandomMove();
        }

        protected override Move GetNextMove(MatchResponse lastMatchResponse)
        {
            // Same move as the move from human the turn before
            return lastMatchResponse.TurnsPlayer2Values[lastMatchResponse.Turn - 1];
        }
    }
}
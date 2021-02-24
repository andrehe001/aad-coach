using System;
using AdventureDay.DataModel;
using AdventureDay.Runner.Model;
using Microsoft.Extensions.Configuration;

namespace AdventureDay.Runner.Players.RealPlayers
{
    public class BetPlayer : RealPlayerBase
    {
        public override string Name => "William";

        public BetPlayer(IConfiguration configuration, Team team, TimeSpan httpTimeout) : base(configuration, team, httpTimeout)
        {
        }

        protected override Move GetFirstMove()
        {
            return GetRandomMove();
        }

        protected override Move GetNextMove(MatchResponse lastMatchResponse)
        {
            if (!lastMatchResponse.Bet.HasValue || lastMatchResponse.Bet.Value == 0)
            {
                throw new MatchCanceledException("Smoorgh has canceled the game, because the stakes from humans are too low");
            }
            
            return GetRandomMove();
        }
    }
}
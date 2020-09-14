using System;
using AdventureDayRunner.Model;
using team_management_api.Data;

namespace AdventureDayRunner.Players.RealPlayers
{
    public class BetPlayer :RealPlayerBase
    {
        public BetPlayer(Team team, TimeSpan httpTimeout) : base(team, httpTimeout)
        {
        }

        public override string Name => "William";
        
        protected override Move GetFirstMove()
        {
            throw new NotImplementedException();
        }

        protected override Move GetNextMove(MatchResponse lastMatchResponse)
        {
            throw new NotImplementedException();
        }
    }
}
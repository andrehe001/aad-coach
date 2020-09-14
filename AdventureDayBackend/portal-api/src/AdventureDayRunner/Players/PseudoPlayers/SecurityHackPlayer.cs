using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using team_management_api.Data;

namespace AdventureDayRunner.Players.PseudoPlayers
{
    public class SecurityHackPlayer : PseudoPlayerBase
    {
        public SecurityHackPlayer(Team team, TimeSpan httpTimeout) : base(team, httpTimeout)
        {
        }

        public override string Name => "Kevin";
        
        protected override Task<MatchReport> ExecuteAction(Team team, HttpClient httpClient, CancellationToken cancellationToken)
        {
            // Run a nasty HTTP Call to the GameEngine Exploit...
            return Task.FromResult(MatchReport.FromHackerAttack(hasDefendedAttack: false));
        }
    }
}
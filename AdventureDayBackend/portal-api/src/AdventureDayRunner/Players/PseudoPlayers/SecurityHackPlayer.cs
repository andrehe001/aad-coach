using System;
using System.Net.Http;
using System.Net.Http.Json;
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
        
        protected override async Task<MatchReport> ExecuteAction(Team team, HttpClient httpClient, CancellationToken cancellationToken)
        {
            var gameEngineSidecarUriBuilder = new UriBuilder(team.GameEngineUri);
            gameEngineSidecarUriBuilder.Port = 81;
            gameEngineSidecarUriBuilder.Path = "Exploit";

            var gameEngineSidecarUri = gameEngineSidecarUriBuilder.Uri;

            try
            {
                var result = await httpClient.GetAsync(gameEngineSidecarUri, cancellationToken: cancellationToken);
                if (result.IsSuccessStatusCode)
                {
                    var response = await result.Content.ReadFromJsonAsync<dynamic>(cancellationToken: cancellationToken);

                    if (response.meshconfigExploited || response.podListExploited)
                    {
                        return MatchReport.FromHackerAttack(hasDefendedAttack: false);
                    }
                }
            }
            catch (Exception)
            {
            }

            return MatchReport.FromHackerAttack(hasDefendedAttack: true);
        }
    }
}
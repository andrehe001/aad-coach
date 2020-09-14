using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;
using team_management_api.Data;

namespace AdventureDayRunner.Players.PseudoPlayers
{
    public class SecurityHackPlayer : PseudoPlayerBase
    {
        private readonly IConfiguration _configuration;

        public SecurityHackPlayer(IConfiguration configuration, Team team, TimeSpan httpTimeout) : base(configuration, team, httpTimeout)
        {
            _configuration = configuration;
        }

        public override string Name => "Kevin";
        
        protected override async Task<MatchReport> ExecuteAction(Team team, HttpClient httpClient, CancellationToken cancellationToken)
        {
            Uri gameEngineSidecarUri; 
            // Develop Mode?
            var localSideCarExploitUri = _configuration.GetValue("LocalSideCarExploitUri", string.Empty);
            if (localSideCarExploitUri != string.Empty)
            {
                Log.Debug($"SecurityHackPlayer: Using LocalSideCarExploitUri {localSideCarExploitUri}");
                gameEngineSidecarUri = new Uri(localSideCarExploitUri);                
            } 
            else
            {
                Log.Debug("SecurityHackPlayer: Using default side car URI.");
                var gameEngineSidecarUriBuilder = new UriBuilder(team.GameEngineUri);
                gameEngineSidecarUriBuilder.Port = 81;
                gameEngineSidecarUriBuilder.Path = "Exploit";
                gameEngineSidecarUri = gameEngineSidecarUriBuilder.Uri;
            }

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
            catch (Exception exception)
            {
                Log.Error(exception, $"Error in Hacker.");
            }

            return MatchReport.FromHackerAttack(hasDefendedAttack: true);
        }
    }
}
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using AdventureDayRunner.Model;
using Microsoft.Extensions.Configuration;
using Serilog;
using team_management_api.Data;

namespace AdventureDayRunner.Players
{
    public abstract class RealPlayerBase : IPlayer
    {
        private readonly IConfiguration _configuration;
        private readonly Team _team;
        private readonly TimeSpan _httpTimeout;
        
        protected RealPlayerBase(IConfiguration configuration, Team team, TimeSpan httpTimeout)
        {
            _configuration = configuration;
            _team = team;
            _httpTimeout = httpTimeout;
        }

        public abstract string Name { get; }
        
        /// <summary>
        /// Returns the final match response. All internal play steps are handled by
        /// the player's "AI" ;-)
        /// </summary>
        /// <param name="cancellationToken">Cancel.</param>
        /// <returns>The final match response or null upon failure.</returns>
        public async Task<MatchReport> Play(CancellationToken cancellationToken)
        {
            using var httpClient = new HttpClient() { Timeout = _httpTimeout };

            var matchRequest = new InitialMatchRequest()
            {
                ChallengerId = Name,
                Move = GetFirstMove()
            };

            var result = await httpClient
                .PostAsJsonAsync(_team.GameEngineUri, matchRequest,
                    cancellationToken: cancellationToken);

            if (!result.IsSuccessStatusCode)
            {
                Log.Error($"{_team.Name} - Status: {result.StatusCode}");
                return MatchReport.FromError($"Status: {result.StatusCode}");
            }

            var response = await result.Content.ReadFromJsonAsync<MatchResponse>(cancellationToken: cancellationToken);
            while (response.MatchOutcome == null)
            {
                var continueMatchRequest = response.ToContinueMatchRequest();
                continueMatchRequest.Move = GetNextMove(response);
                var subSeqResult = await httpClient
                    .PostAsJsonAsync(_team.GameEngineUri, continueMatchRequest,
                        cancellationToken: cancellationToken);

                if (!subSeqResult.IsSuccessStatusCode)
                {
                    return MatchReport.FromError($"Status: {result.StatusCode}");
                }

                response = await subSeqResult.Content.ReadFromJsonAsync<MatchResponse>(
                    cancellationToken: cancellationToken);
            }

            return MatchReport.FromMatchResponse(response);
        }
        protected abstract Move GetFirstMove();
        protected abstract Move GetNextMove(MatchResponse lastMatchResponse);
    }
}
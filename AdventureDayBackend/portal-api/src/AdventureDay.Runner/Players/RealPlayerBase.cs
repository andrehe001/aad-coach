using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using AdventureDay.Runner.Model;
using Microsoft.Extensions.Configuration;
using Serilog;
using AdventureDay.ManagementApi.Data;

namespace AdventureDay.Runner.Players
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
                // TODO: Differentiate between 500s and others (bet error)
                Log.Information($"Team: {_team.Name} (ID: {_team.Id}) PlayerName: {Name} - REQ Status: {result.StatusCode}");
                return MatchReport.FromError($"Smoorghs are receiving the HTTP status code of {result.StatusCode}");
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
                    // TODO: Differentiate between 500s and others (bet error)
                    Log.Information($"Team: {_team.Name} (ID: {_team.Id}) PlayerName: {Name} - REQ Status: {subSeqResult.StatusCode}");
                    return MatchReport.FromError($"Smoorghs are receiving the HTTP status code of {subSeqResult.StatusCode}");
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
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using AdventureDayRunner.Model;
using Serilog;
using team_management_api.Data;

namespace AdventureDayRunner.Players
{
    public abstract class RealPlayerBase : IPlayer
    {
        private readonly Team _team;
        private readonly TimeSpan _httpTimeout;
        
        protected RealPlayerBase(Team team, TimeSpan httpTimeout)
        {
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
        public async Task<MatchScoreReport> Play(CancellationToken cancellationToken)
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
                return null;
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
                    return null;
                }

                response = await subSeqResult.Content.ReadFromJsonAsync<MatchResponse>(
                    cancellationToken: cancellationToken);
            }

            return MatchScoreReport.FromMatchResponse(response);
        }
        protected abstract Move GetFirstMove();
        protected abstract Move GetNextMove(MatchResponse lastMatchResponse);
    }
}
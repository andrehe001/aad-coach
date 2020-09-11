using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using AdventureDayRunner.Model;
using AdventureDayRunner.Shared;
using Serilog;

namespace AdventureDayRunner.Players
{
    public abstract class PlayerBase
    {
        private readonly AdventureDayTeamInformation _teamInformation;
        private readonly TimeSpan _httpTimeout;
        
        protected PlayerBase(AdventureDayTeamInformation teamInformation, TimeSpan httpTimeout)
        {
            _teamInformation = teamInformation;
            _httpTimeout = httpTimeout;
        }

        /// <summary>
        /// Returns the final match response. All internal play steps are handled by
        /// the player's "AI" ;-)
        /// </summary>
        /// <param name="cancellationToken">Cancel.</param>
        /// <returns>The final match response or null upon failure.</returns>
        public async Task<MatchResponse> Play(CancellationToken cancellationToken)
        {
            MatchResponse response;
            using var httpClient = new HttpClient() { Timeout = _httpTimeout };

            try
            {
                var matchRequest = new InitialMatchRequest()
                {
                    ChallengerId = Name,
                    Move = GetFirstMove()
                };

                var result = await httpClient
                    .PostAsJsonAsync(_teamInformation.GameEngineUri, matchRequest,
                        cancellationToken: cancellationToken);

                if (!result.IsSuccessStatusCode)
                {
                    Log.Error($"{_teamInformation.Name} - Status: {result.StatusCode}");
                    return null;
                }

                response = await result.Content.ReadFromJsonAsync<MatchResponse>(cancellationToken: cancellationToken);
                while (response.MatchOutcome == null)
                {
                    var continueMatchRequest = response.ToContinueMatchRequest();
                    continueMatchRequest.Move = GetNextMove(response);
                    var subSeqResult = await httpClient
                        .PostAsJsonAsync(_teamInformation.GameEngineUri, continueMatchRequest,
                            cancellationToken: cancellationToken);

                    if (!subSeqResult.IsSuccessStatusCode)
                    {
                        return null;
                    }

                    response = await subSeqResult.Content.ReadFromJsonAsync<MatchResponse>(
                        cancellationToken: cancellationToken);
                }
            }
            catch (TaskCanceledException)
            {
                // If it wasn't our own cancellation -> http timer triggered.
                if (!cancellationToken.IsCancellationRequested)
                {
                    Log.Error("HTTP Timeout.");
                }

                return null;
            }
            catch(Exception ex)
            {
                Log.Error(ex, "Error.");
                return null;
            }

            return response;
        }
        protected abstract string Name { get; }

        protected abstract Move GetFirstMove();
        protected abstract Move GetNextMove(MatchResponse lastMatchResponse);
    }
}
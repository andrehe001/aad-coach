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
        private readonly string _name;
        
        protected PlayerBase(AdventureDayTeamInformation teamInformation, TimeSpan httpTimeout)
        {
            _name = Utils.GenerateName();
            _teamInformation = teamInformation;
            _httpTimeout = httpTimeout;
        }

        public async Task<MatchResponse> Play(CancellationToken cancellationToken)
        {
            var seq = 0;
            MatchResponse response;
            using var httpClient = new HttpClient() { Timeout = _httpTimeout };

            try
            {
                var matchRequest = new InitialMatchRequest()
                {
                    ChallengerId = _name,
                    Move = GetNextMove(seq++)
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
                    continueMatchRequest.Move = GetNextMove(seq++);
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
 
        protected abstract Move GetNextMove(int sequenceNumber);
    }
}
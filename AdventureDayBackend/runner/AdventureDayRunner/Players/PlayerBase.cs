using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Serilog;

namespace AdventureDayRunner.Players
{
    public abstract class PlayerBase
    {
        protected PlayerBase(string uri)
        {
            Name = Utils.GenerateName();
            Id = System.Guid.NewGuid().ToString();
            Uri = uri;
        }

        public string Name { get; set; }
        public string Id { get; set; }

        private string Uri;

        public async Task<MatchStatistic> Play(CancellationToken cancellationToken)
        {
            MatchStatistic statistic = new MatchStatistic();
            do
            {
                statistic = await SendMove(statistic);
            } while (statistic.MatchOutcome == null);
            return statistic;
        }
 
        private async Task<MatchStatistic> SendMove(MatchStatistic statisticSoFar)
        {
            var matchRequestParameter = new Dictionary<string, string>
            {
                {"ChallengerId", Name},
                {"Move", GetNextMove(statisticSoFar).ToString()}
            };
            
            if (statisticSoFar.MatchId != null) {
                matchRequestParameter.Add("MatchId", statisticSoFar.MatchId.ToString());
            }
            
            Log.Information($"Player {Name} sends request against {Uri}");
            String response = await Utils.SendMatchRequest(Uri, matchRequestParameter);
            
            var serializeOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            
            MatchStatistic statistic = JsonSerializer.Deserialize<MatchStatistic>(response, serializeOptions);
            return statistic;
        }

        protected abstract Move GetNextMove(MatchStatistic historicMatchStatistics);
    }
}
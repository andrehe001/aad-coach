using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace AdventureDayRunner
{
    public abstract class Player
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        public Player(string uri)
        {
            Name = Utils.GenerateName();
            Id = System.Guid.NewGuid().ToString();
            Uri = uri;
        }

        public string Name { get; set; }
        public string Id { get; set; }

        private string Uri;

        public async Task<MatchStatistic> Play()
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
            var matchRequestParameter = new Dictionary<string, string>();
            matchRequestParameter.Add("ChallengerId", Name);
            matchRequestParameter.Add("Move", getNextMove(statisticSoFar).ToString());
            if (statisticSoFar.MatchId != null) {
                matchRequestParameter.Add("MatchId", statisticSoFar.MatchId.ToString());
            }
            Logger.Info(string.Format("Player {0} sends request against {1}", Name, Uri));
            String response = await Utils.SendMatchRequest(Uri, matchRequestParameter);
            
            var serializeOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            MatchStatistic statistic = JsonSerializer.Deserialize<MatchStatistic>(response, serializeOptions);
            return statistic;
        }

        protected abstract Move getNextMove(MatchStatistic statisticSoFar);
    }
}
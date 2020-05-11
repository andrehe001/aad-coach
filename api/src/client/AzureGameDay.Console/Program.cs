using System;
using AzureGameDay.SDK.Model;
using Newtonsoft.Json;

namespace AzureGameDay.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var challengerId = Guid.NewGuid().ToString();
            var matchSetupApi = new SDK.Api.MatchSetupApi();
            var matchApi = new SDK.Api.MatchApi();
            
            var matchSetup = matchSetupApi.MatchSetupPost(new MatchSetupRequest() { ChallengerId = challengerId });
            var match = matchApi.MatchPost(new MatchRequest()
            {
                ChallengerId = challengerId,
                MatchId = matchSetup.MatchId,
                MatchSequenceNumber = matchSetup.MatchSequenceNumber, 
                Move = Move.Spock
            });
            
            System.Console.WriteLine($"Match Result:");
            System.Console.WriteLine($"{JsonConvert.SerializeObject(match, Formatting.Indented)}");
        }
    }
}

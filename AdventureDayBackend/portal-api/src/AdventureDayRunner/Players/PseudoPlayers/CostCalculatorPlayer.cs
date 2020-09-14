using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using team_management_api.Data;

namespace AdventureDayRunner.Players.PseudoPlayers
{
    public class CostCalculatorPlayer : PseudoPlayerBase
    {
        public CostCalculatorPlayer(Team team, TimeSpan httpTimeout) : base(team, httpTimeout)
        {
        }
        
        public override string Name => "Dagobert";
        
        protected override Task<MatchReport> ExecuteAction(Team team, HttpClient httpClient, CancellationToken cancellationToken)
        {
            return Task.FromResult(MatchReport.FromCostCalculator(0, 1));
        }
    }
}
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using team_management_api.Data;

namespace AdventureDayRunner.Players
{
    public abstract class PseudoPlayerBase : IPlayer
    {
        private readonly Team _team;
        private readonly TimeSpan _httpTimeout;

        protected PseudoPlayerBase(Team team, TimeSpan httpTimeout)
        {
            _team = team;
            _httpTimeout = httpTimeout;
        }

        public Task<MatchReport> Play(CancellationToken cancellationToken)
        {
            using var httpClient = new HttpClient() { Timeout = _httpTimeout };
            return ExecuteAction(_team, httpClient, cancellationToken);
        }

        public abstract string Name { get; }

        protected abstract Task<MatchReport> ExecuteAction(
            Team team,
            HttpClient httpClient,
            CancellationToken cancellationToken);
    }
}
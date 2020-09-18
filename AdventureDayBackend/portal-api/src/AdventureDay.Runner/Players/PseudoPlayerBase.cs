using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using AdventureDay.ManagementApi.Data;

namespace AdventureDay.Runner.Players
{
    public abstract class PseudoPlayerBase : IPlayer
    {
        private readonly IConfiguration _configuration;
        private readonly Team _team;
        private readonly TimeSpan _httpTimeout;

        protected PseudoPlayerBase(IConfiguration configuration, Team team, TimeSpan httpTimeout)
        {
            _configuration = configuration;
            _team = team;
            _httpTimeout = httpTimeout;
        }

        public async Task<MatchReport> Play(CancellationToken cancellationToken)
        {
            using var httpClient = new HttpClient() { Timeout = _httpTimeout };
            var result = await ExecuteAction(_team, httpClient, cancellationToken);
            return result;
        }

        public abstract string Name { get; }

        protected abstract Task<MatchReport> ExecuteAction(
            Team team,
            HttpClient httpClient,
            CancellationToken cancellationToken);
    }
}
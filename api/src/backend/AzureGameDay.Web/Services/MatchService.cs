using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AzureGameDay.Web.Models;

namespace AzureGameDay.Web.Services
{
    public class MatchService
    {
        private readonly IOverlordStrategy _overlordStrategy;
        
        // TODO Use something real and not a half-ass locked in mem KV.
        private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1,1);
        private readonly Dictionary<Guid, long> _matchSequenceNumbers = new Dictionary<Guid, long>();
        private readonly Dictionary<Guid, List<MatchSetup>> _matchSetups = new Dictionary<Guid, List<MatchSetup>>();
        private readonly Dictionary<Guid, List<Match>> _matches = new Dictionary<Guid, List<Match>>();

        public MatchService(IOverlordStrategy overlordStrategy)
        {
            _overlordStrategy = overlordStrategy;
        }

        public async Task<MatchSetup> SetupMatch(MatchSetupRequest matchSetupRequest)
        {
            var challengerId = matchSetupRequest.ChallengerId;
            var matchSetup = new MatchSetup()
            {
                ChallengerId = challengerId,
                MatchId = Guid.NewGuid(),
            };
            
            await _semaphoreSlim.WaitAsync();
            try
            {
                
                if (!_matchSetups.ContainsKey(challengerId))
                {
                    _matchSetups[challengerId] = new List<MatchSetup>();
                }

                if (!_matchSequenceNumbers.ContainsKey(challengerId))
                {
                    _matchSequenceNumbers[challengerId] = 0;
                }

                _matchSequenceNumbers[challengerId] = _matchSequenceNumbers[challengerId] + 1;
                matchSetup.MatchSequenceNumber = _matchSequenceNumbers[challengerId];
                _matchSetups[challengerId].Add(matchSetup);
            }
            finally
            {
                _semaphoreSlim.Release();
            }

            return matchSetup;
        }

        public Task<IEnumerable<Match>> GetChallengerMatches(Guid challengerId)
        {
            var hasValue = _matches.TryGetValue(challengerId, out List<Match> challengerMatches);
            return Task.FromResult(hasValue ? challengerMatches : Enumerable.Empty<Match>());
        }
        
        public async Task<Match> PlayMatch(MatchRequest matchRequest)
        {
            await _semaphoreSlim.WaitAsync();
            try
            {
                if (!_matchSetups.TryGetValue(matchRequest.ChallengerId, out List<MatchSetup> challengerMatchSetups))
                {
                    return null;
                }

                var matchSetup = challengerMatchSetups.FirstOrDefault(m => 
                    m.MatchId == matchRequest.MatchId 
                    && m.MatchSequenceNumber == matchRequest.MatchSequenceNumber);
                if (matchSetup == null)
                {
                    return null;
                }

                challengerMatchSetups.Remove(matchSetup);
                
                var res = Match.CreateNewFromMatchSetup(matchSetup);
                res.ChallengerMove = matchRequest.Move;
                res.OverlordMove = _overlordStrategy.NextMove(matchRequest.ChallengerId);
                res.Outcome = CalculateResult(matchRequest.Move, res.OverlordMove);
                if (!_matches.ContainsKey(res.ChallengerId))
                {
                    _matches.Add(res.ChallengerId, new List<Match>());
                }
                _matches[res.ChallengerId].Add(res);
                return res;
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        private Outcome CalculateResult(Move challengerMove, Move overlordMove)
        {
            if (challengerMove == overlordMove)
            {
                return Outcome.Tie;
            }

            return challengerMove switch
            {
                Move.Rock => overlordMove == Move.Lizard || overlordMove == Move.Scissors
                    ? Outcome.ChallengerWins
                    : Outcome.OverlordWins,
                Move.Paper => overlordMove == Move.Spock || overlordMove == Move.Rock
                    ? Outcome.ChallengerWins
                    : Outcome.OverlordWins,
                Move.Scissors => overlordMove == Move.Paper || overlordMove == Move.Lizard
                    ? Outcome.ChallengerWins
                    : Outcome.OverlordWins,
                Move.Lizard => overlordMove == Move.Spock || overlordMove == Move.Paper
                    ? Outcome.ChallengerWins
                    : Outcome.OverlordWins,
                Move.Spock => overlordMove == Move.Scissors || overlordMove == Move.Rock
                    ? Outcome.ChallengerWins
                    : Outcome.OverlordWins,
                _ => throw new NotImplementedException()
            };
        }
    }
}
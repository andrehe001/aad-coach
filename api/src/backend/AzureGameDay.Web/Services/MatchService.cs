using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AzureGameDay.Web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace AzureGameDay.Web.Services
{
    public class MatchService
    {
        private readonly IOverlordStrategy _overlordStrategy;
        private readonly IDistributedCache _cache;
        private readonly MatchDBContext _dbContext;
        private readonly IConfiguration _config;

        // TODO Use something real and not a half-ass locked in mem KV.
        private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);
        private readonly Dictionary<Guid, long> _matchSequenceNumbers = new Dictionary<Guid, long>();
        private readonly Dictionary<Guid, List<MatchSetup>> _matchSetups = new Dictionary<Guid, List<MatchSetup>>();
        private readonly Dictionary<Guid, List<Match>> _matches = new Dictionary<Guid, List<Match>>();


        public MatchService(IDistributedCache cache, MatchDBContext dbContext, IConfiguration config)

        {

            _cache = cache;
            _dbContext = dbContext;
            _config = config;
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

            var currentMatch = (matchRequest.MatchId != Guid.Empty) ? await GetMatchFromCacheAsync(matchRequest.MatchId) : Match.CreateNewFromMatchRequest(matchRequest);
            if (currentMatch.MatchOutcome!=null)
            {
                // game was already finished
                throw new Exception("this game is already over.");
            }
            currentMatch.TurnsPlayer1Values.Add(matchRequest.Move);

            // get move from bot
            var botMove = await GetBotMoveAsync(new GameInfoForBackend { Challenger = matchRequest.ChallengerId, MatchId = matchRequest.MatchId });
            currentMatch.TurnsPlayer2Values.Add(botMove);

            currentMatch.LastRoundOutcome = CalculateResult(matchRequest.Move, botMove);
            currentMatch.Turn++;


            var matchwinner = CalculateMatchWinner(currentMatch);

            currentMatch.MatchOutcome = matchwinner;

            // save current match state in cache
            SaveMatchToCache(currentMatch);

            // if game over store to db
            if (currentMatch.MatchOutcome != null)
            {
                StoreMatchToDB(currentMatch);   
            }

            return currentMatch;

        }

        private Outcome? CalculateMatchWinner(Match currentMatch)
        {
            // check for winner
            var turnsCounter = 0;
            var turnsWonByPlayer1 = 0;
            var turnsWonByPlayer2 = 0;
            foreach (var item in currentMatch.TurnsPlayer1Values)
            {
                switch (CalculateResult(item, currentMatch.TurnsPlayer2Values[turnsCounter++]))
                {
                    case Outcome.ChallengerWins:
                        turnsWonByPlayer1++;
                        break;

                    case Outcome.OverlordWins:
                        turnsWonByPlayer2++;
                        break;
                    default: // Tie
                        break;
                }
            }
            if (turnsCounter > 3)
            {
                // error 
                throw new Exception("There can only be 3 turns in one match." + currentMatch);
            }

            if (turnsCounter == 3) // three rounds, there must be a winner
            {
                return (turnsWonByPlayer1 > turnsWonByPlayer2) ? Outcome.ChallengerWins : Outcome.OverlordWins;
            }
            if (turnsCounter == 2) // two rounds have been played, there might be a winner
            {
                if (turnsWonByPlayer1 == 2) return Outcome.ChallengerWins;
                if (turnsWonByPlayer2 == 2) return Outcome.OverlordWins;
            }
            // only one round has been played, there can't be a winner
            return null;
        }

        private void StoreMatchToDB(Match currentMatch)
        {
            _dbContext.MatchResults.Add(new MatchResult
            {
                MatchId = currentMatch.MatchId,
                WhenUtc = currentMatch.WhenUtc,
                MatchOutcome = currentMatch.MatchOutcome.ToString(),
                MatchSequenceNumber = currentMatch.MatchSequenceNumber,
                Player1Name = currentMatch.Player1Name,
                Player2Name = currentMatch.Player2Name
            }
                );

            int i = 0;
            foreach (var item in currentMatch.TurnsPlayer1Values)
            {
                _dbContext.Turns.Add(new Turn
                {
                    MatchId = currentMatch.MatchId,
                    WhenUtc = currentMatch.WhenUtc,
                    Player1Name = currentMatch.Player1Name,
                    Player2Name = currentMatch.Player2Name,
                    Player1Move = item.ToString(),
                    TurnNumber = i,
                    Player2Move = currentMatch.TurnsPlayer2Values[i++].ToString(),
                });
            }

            _dbContext.SaveChanges();

        }

        private void SaveMatchToCache(Match m)
        {
            string serializedMatch = JsonConvert.SerializeObject(m);
            _cache.SetStringAsync(m.MatchId.ToString(), serializedMatch);

        }

        private async Task<Match> GetMatchFromCacheAsync(Guid matchId)
        {
            var o = await _cache.GetStringAsync(matchId.ToString());
            Match item = JsonConvert.DeserializeObject<Match>(o);
            return item;
        }

        private async Task<Move> GetBotMoveAsync(GameInfoForBackend gameInfoForBackend)
        {
            //todo: reach out to backend service of arcade team to get move; send gameid, challengerid, turn
            //string backendurl = _config.GetValue<string>("ARCADE_BACKENDURL");
            //HttpClient cl = new HttpClient();
            //var content = new StringContent(JsonConvert.SerializeObject(gameInfoForBackend), Encoding.UTF8, "application/json");
            //var res = await cl.PostAsync(backendurl, content);
            //return JsonConvert.DeserializeObject<MoveDTO>(await res.Content.ReadAsStringAsync()).Move;


            return Move.Lizard;
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
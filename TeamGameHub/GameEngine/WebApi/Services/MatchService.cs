using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TeamGameHub.GameEngine.WebApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Runtime.CompilerServices;

namespace TeamGameHub.GameEngine.WebApi.Services
{
    public class MatchService
    {
        private readonly ILogger<MatchService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IDistributedCache _cache;
        private readonly MatchDBContext _dbContext;
        private readonly IConfiguration _config;
        private readonly Lazy<bool> _useMockBot;
        private readonly JsonSerializerOptions _stringEnumConverterOptions;
        private readonly Dictionary<Guid, List<Match>> _matches = new Dictionary<Guid, List<Match>>();

        private TelemetryClient _telemetry;

        // EncryptionKey
        public static string _eKey { get; private set; }

        public MatchService(ILogger<MatchService> logger, IConfiguration configuration, IDistributedCache cache, MatchDBContext dbContext, IConfiguration config, TelemetryClient telemetry)

        {
            _eKey = "asdfbaasdfjknasere456789";
            _logger = logger;
            _configuration = configuration;
            _cache = cache;
            _dbContext = dbContext;
            _config = config;
            _telemetry = telemetry;

            _useMockBot = new Lazy<bool>(() => _configuration.GetValue<bool>("UseMockBot", false));

            _stringEnumConverterOptions = new JsonSerializerOptions
            {
                Converters ={
                        new JsonStringEnumConverter()
                 }
            };
        }

        public Task<IEnumerable<Match>> GetChallengerMatches(Guid challengerId)
        {
            bool hasValue = _matches.TryGetValue(challengerId, out List<Match> challengerMatches);
            return Task.FromResult(hasValue ? challengerMatches : Enumerable.Empty<Match>());
        }

        [MethodImpl(MethodImplOptions.NoOptimization | MethodImplOptions.NoInlining)]
        private void WasteIOandCPUandRAM()
        {
            // Waste SQL: TODO find schema issue.
            _dbContext.Database.ExecuteSqlRaw("SELECT TOP 500 * FROM [dbo].[MatchResults] ORDER BY WhenUtc");
            _dbContext.Database.ExecuteSqlRaw("SELECT TOP 500 * FROM [dbo].[MatchResults] ORDER BY WhenUtc");
            _dbContext.Database.ExecuteSqlRaw("SELECT TOP 500 * FROM [dbo].[MatchResults] ORDER BY WhenUtc");
            _dbContext.Database.ExecuteSqlRaw("SELECT TOP 500 * FROM [dbo].[MatchResults] ORDER BY WhenUtc");
            _dbContext.Database.ExecuteSqlRaw("SELECT TOP 500 * FROM [dbo].[MatchResults] ORDER BY WhenUtc");
            _dbContext.Database.ExecuteSqlRaw("SELECT TOP 500 * FROM [dbo].[MatchResults] ORDER BY WhenUtc");
            _dbContext.Database.ExecuteSqlRaw("SELECT TOP 500 * FROM [dbo].[MatchResults] ORDER BY WhenUtc");
            _dbContext.Database.ExecuteSqlRaw("SELECT TOP 500 * FROM [dbo].[MatchResults] ORDER BY WhenUtc");
            _dbContext.Database.ExecuteSqlRaw("SELECT TOP 500 * FROM [dbo].[MatchResults] ORDER BY WhenUtc");
            _dbContext.Database.ExecuteSqlRaw("SELECT TOP 500 * FROM [dbo].[MatchResults] ORDER BY WhenUtc");

            // Waste CPU
            var end = DateTime.Now + TimeSpan.FromSeconds(2);
            var result = 0;
            while (DateTime.Now < end)
            {
                result += 1;
                result -= 1;
                result += 400;
                result *= 500;
                result /= 500;
            }
            Console.WriteLine(result.ToString());
        }

        public async Task<Match> PlayMatch(MatchRequest matchRequest)
        {
            _logger.LogInformation("Match request received from ${matchRequest.ChallengerId}");

            // If MassPlayer
            if (matchRequest.ChallengerId == "Gloria")
            {
                WasteIOandCPUandRAM();
            }

            Match currentMatch = (matchRequest.MatchId != Guid.Empty)
                ? await GetMatchFromCacheAsync(matchRequest.MatchId)
                : Match.CreateNewFromMatchRequest(matchRequest);

            if (currentMatch.MatchOutcome != null)
            {
                // game was already finished
                throw new Exception("this game is already over.");
            }

            // send matchinfo to backend and get move from bot.
            // Important! Do this before you set the value of Player1!
            MoveDTO botMove = await GetBotMoveAsync(currentMatch);

            if (currentMatch.Turn == 0)
            {
                if (botMove.Bet.HasValue && (botMove.Bet < 0 || botMove.Bet > 1))
                {
                    throw new Exception("The bet can only be between (inclusive) 0 and 1.");
                }
                currentMatch.Bet = botMove.Bet;
            }
            // TODO: activate this again but the Bot needs to differentiate between first Pick and the other 1-2 picks
            //else if (currentMatch.Bet != botMove.Bet)
            //{
            //    // throw new Exception("The bet can only be set at the beginning and not changed afterwards.");
            //}

            currentMatch.TurnsPlayer1Values.Add(matchRequest.Move);
            currentMatch.TurnsPlayer2Values.Add(botMove.Move);

            currentMatch.LastRoundOutcome = CalculateResult(matchRequest.Move, botMove.Move);
            currentMatch.Turn++;

            Outcome? matchwinner = CalculateMatchWinner(currentMatch);

            currentMatch.MatchOutcome = matchwinner;

            // save current match state in cache
            await SaveMatchToCache(currentMatch);

            // if game over store to db
            if (currentMatch.MatchOutcome != null)
            {
                StoreMatchToDB(currentMatch);

                try{
                    if (_telemetry != null &&_telemetry.IsEnabled()) {
                        _telemetry.Context.Cloud.RoleName = "engine";
                        var properties = new Dictionary<string,string>();
                        properties.Add("player", matchRequest.ChallengerId );
                        properties.Add("move", matchRequest.Move.ToString() );
                        properties.Add("botmove", botMove.Move.ToString() );
                        properties.Add("outcome", currentMatch.MatchOutcome.ToString() );
                        properties.Add("matchId", matchRequest.MatchId.ToString() );
                        properties.Add("bet", (currentMatch.Bet.HasValue)? currentMatch.Bet.Value.ToString() : "0" );
                        properties.Add("player1", currentMatch.Player1Name );
                        properties.Add("player2", currentMatch.Player2Name );
                        _telemetry.TrackEvent("match", properties, null);
                    }
                }catch(Exception ex){
                    Console.WriteLine(ex);
                }
            }

            return currentMatch;
        }

        private Outcome? CalculateMatchWinner(Match currentMatch)
        {
            // check for winner
            int turnsCounter = 0;
            int turnsWonByPlayer1 = 0;
            int turnsWonByPlayer2 = 0;
            foreach (Move item in currentMatch.TurnsPlayer1Values)
            {
                switch (CalculateResult(item, currentMatch.TurnsPlayer2Values[turnsCounter++]))
                {
                    case Outcome.SmoorghWins:
                        turnsWonByPlayer1++;
                        break;

                    case Outcome.HumanBotWins:
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
                return (turnsWonByPlayer1 > turnsWonByPlayer2) ? Outcome.SmoorghWins : Outcome.HumanBotWins;
            }
            if (turnsCounter == 2) // two rounds have been played, there might be a winner
            {
                if (turnsWonByPlayer1 == 2)
                {
                    return Outcome.SmoorghWins;
                }

                if (turnsWonByPlayer2 == 2)
                {
                    return Outcome.HumanBotWins;
                }
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
                Player1Name = currentMatch.Player1Name,
                Player2Name = currentMatch.Player2Name
            });

            int i = 0;
            foreach (Move item in currentMatch.TurnsPlayer1Values)
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

        private async Task SaveMatchToCache(Match m)
        {
            string serializedMatch = JsonSerializer.Serialize(m);
            string encryptedString = Encrypt(serializedMatch);
            await _cache.SetStringAsync(m.MatchId.ToString(), encryptedString);
        }

        private async Task<Match> GetMatchFromCacheAsync(Guid matchId)
        {
            string o = await _cache.GetStringAsync(matchId.ToString());
            string decryptedString = Decrypt(o);
            Match item = JsonSerializer.Deserialize<Match>(decryptedString);
            return item;
        }

        public static string Encrypt(string clearText)
        {
            string EncryptionKey = _eKey;
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes rdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = rdb.GetBytes(32);
                encryptor.IV = rdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
        public static string Decrypt(string cipherText)
        {
            string EncryptionKey = _eKey;
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes rdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = rdb.GetBytes(32);
                encryptor.IV = rdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        private async Task<MoveDTO> GetBotMoveAsync(Match gameInfoForBackend)
        {
            if (_useMockBot.Value)
            {
                _logger.LogInformation("Using mock bot.");
                return new MoveDTO() { Move = Move.Rock };
            }

            string backendurl = _config.GetValue<string>("ARCADE_BACKENDURL");

            HttpClient cl = new HttpClient();
            StringContent content = new StringContent(System.Text.Json.JsonSerializer.Serialize(gameInfoForBackend, _stringEnumConverterOptions),
                Encoding.UTF8, "application/json");
            HttpResponseMessage res = await cl.PostAsync(backendurl, content);            
            return JsonSerializer.Deserialize<MoveDTO>(await res.Content.ReadAsStringAsync(), _stringEnumConverterOptions);
        }

        private Outcome CalculateResult(Move smoorghMove, Move humanBotMove)
        {
            if (smoorghMove == humanBotMove)
            {
                return Outcome.Tie;
            }

            return smoorghMove switch
            {
                Move.Rock => humanBotMove == Move.Metal || humanBotMove == Move.Scissors
                    ? Outcome.SmoorghWins
                    : Outcome.HumanBotWins,
                Move.Paper => humanBotMove == Move.Snap || humanBotMove == Move.Rock
                    ? Outcome.SmoorghWins
                    : Outcome.HumanBotWins,
                Move.Scissors => humanBotMove == Move.Paper || humanBotMove == Move.Metal
                    ? Outcome.SmoorghWins
                    : Outcome.HumanBotWins,
                Move.Metal => humanBotMove == Move.Snap || humanBotMove == Move.Paper
                    ? Outcome.SmoorghWins
                    : Outcome.HumanBotWins,
                Move.Snap => humanBotMove == Move.Scissors || humanBotMove == Move.Rock
                    ? Outcome.SmoorghWins
                    : Outcome.HumanBotWins,
                _ => throw new NotImplementedException()
            };
        }
    }
}
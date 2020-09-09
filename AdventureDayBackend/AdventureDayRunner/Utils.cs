using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Authentication;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AdventureDayRunner.Shared;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Serilog;

namespace AdventureDayRunner
{
    public class Utils
    {
        private static readonly HttpClient client = new HttpClient();
        private static Lazy<IConfiguration> configuration;

        static Utils()
        {
            configuration = new Lazy<IConfiguration>(BuildConfiguration);
        }

        public static string GenerateName(int len = 5)
        {
            Random r = new Random();
            string[] consonants =
            {
                "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "l", "n", "p", "q", "r", "s", "sh", "zh", "t", "v",
                "w", "x"
            };
            string[] vowels = {"a", "e", "i", "o", "u", "ae", "y"};
            string Name = "";
            Name += consonants[r.Next(consonants.Length)].ToUpper();
            Name += vowels[r.Next(vowels.Length)];
            int
                b = 2; //b tells how many times a new letter has been added. It's 2 right now because the first two letters are already in the name.
            while (b < len)
            {
                Name += consonants[r.Next(consonants.Length)];
                b++;
                Name += vowels[r.Next(vowels.Length)];
                b++;
            }

            return Name;
        }

        #region Configuration

        private static IConfiguration Configuration => configuration.Value;

        private static IConfiguration BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            var config = builder.Build();
            return config;
        }

        #endregion

        internal static async Task<string> SendMatchRequest(String uri,
            Dictionary<string, string> matchRequestParameter)
        {
            string json = JsonSerializer.Serialize(matchRequestParameter);
            var response = await client.PostAsync(uri,
                new StringContent(json, Encoding.UTF8, "application/json"));
            return await response.Content.ReadAsStringAsync();
        }

        public static async Task WriteDefaultPropertiesToDb()
        {
            Log.Information("Writing default properties to DB.");
            var connectionString = Configuration.GetConnectionString("DbConnection");
            var dbName = Configuration.GetSection("Parameter").GetSection("DbName").Value;
            var dbCollectionName = Configuration.GetSection("Parameter").GetSection("DbCollectionName").Value;

            var repo = new AdventureDayPropertiesRepository(new AdventureDayDatabaseSettings() { ConnectionString = connectionString, CollectionName = dbCollectionName, DatabaseName = dbName});
            await repo.CreateAsync(AdventureDayRunnerProperties.Default);
        }


        public static async Task<AdventureDayRunnerProperties> ReadPropertiesFromDb(CancellationToken token)
        {
            var connectionString = Configuration.GetConnectionString("DbConnection");
            var dbName = Configuration.GetSection("Parameter").GetSection("DbName").Value;
            var dbCollectionName = Configuration.GetSection("Parameter").GetSection("DbCollectionName").Value;

            var repo = new AdventureDayPropertiesRepository(new AdventureDayDatabaseSettings() { ConnectionString = connectionString, CollectionName = dbCollectionName, DatabaseName = dbName});

            do
            {
                // TODO: Make doc configurable.
                var document = repo.Get("Default");
                if (document == null)
                {
                    Log.Error($"Found no Properties in configured persistence: ${repo.Settings}");
                    await Task.Delay(TimeSpan.FromSeconds(5), token);
                }
                else
                {
                    return document;
                }
            } while (!token.IsCancellationRequested);
            
            throw new InvalidOperationException("Should never reach this line.");
        }
    }
    
    
}
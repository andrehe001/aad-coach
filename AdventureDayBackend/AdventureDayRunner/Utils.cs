using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Security.Authentication;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AdventureDayRunner
{
    public class Utils
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly HttpClient client = new HttpClient();
        public static string GenerateName(int len=5)
        { 
            Random r = new Random();
            string[] consonants = { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "l", "n", "p", "q", "r", "s", "sh", "zh", "t", "v", "w", "x" };
            string[] vowels = { "a", "e", "i", "o", "u", "ae", "y" };
            string Name = "";
            Name += consonants[r.Next(consonants.Length)].ToUpper();
            Name += vowels[r.Next(vowels.Length)];
            int b = 2; //b tells how many times a new letter has been added. It's 2 right now because the first two letters are already in the name.
            while (b < len)
            {
                Name += consonants[r.Next(consonants.Length)];
                b++;
                Name += vowels[r.Next(vowels.Length)];
                b++;
            }
            return Name;
        }

        internal static async Task<string> SendMatchRequest(String uri, Dictionary<string, string> matchRequestParameter)
        {
            string json = JsonSerializer.Serialize(matchRequestParameter);
            var response = await client.PostAsync(uri, 
                new StringContent(json, Encoding.UTF8, "application/json"));
            return await response.Content.ReadAsStringAsync();
        }

        private static string GetConnectionString()
        {
            var builder = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                                .AddEnvironmentVariables();
 
            return builder.Build().GetConnectionString("DbConnection");
        }

        private static Dictionary<string,string> GetParameters()
        {
            var dict = new Dictionary<string, string>();
            var builder = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                                .AddEnvironmentVariables();
            
            var builtBuilder = builder.Build();
 
            dict.Add("DbName", builtBuilder.GetSection("Parameter").GetSection("DbName").Value);
            dict.Add("DbCollectionName", builtBuilder.GetSection("Parameter").GetSection("DbCollectionName").Value); 
            return dict;
        }

        public static Properties ReadPropertiesFromDb()
        {
            Logger.Info("Reading properties from DB");
            var dict = GetParameters();
            string connectionString = GetConnectionString();
            MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
            settings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
            var mongoClient = new MongoClient(settings);
            IMongoDatabase mongoDatabase = mongoClient.GetDatabase(dict.GetValueOrDefault("DbName", "agdrunnerdb"));
            var mongoCollection = mongoDatabase.GetCollection<BsonDocument>(dict.GetValueOrDefault("DbCollectionName", "runnermeta"));
            var projection = Builders<BsonDocument>.Projection.Exclude("_id");
            var document = mongoCollection.Find(new BsonDocument()).Project(projection).FirstOrDefault();
            return JsonSerializer.Deserialize<Properties>(document.ToJson());
        }
    }
}
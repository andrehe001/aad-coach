using System.Collections.Generic;
using System.IO;
using System.Security.Authentication;
using System.Threading.Tasks;
using GameDayRunner.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;

namespace GameDayRunner
{
    public class Utils
    {
        public static string GetConnectionString()
        {
            var builder = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
 
            return builder.Build().GetConnectionString("DbConnection");
        }

        public static Dictionary<string,string> GetParameters()
        {
            var dict = new Dictionary<string, string>();
            var builder = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            var builtBuilder = builder.Build();
 
            dict.Add("DbName", builtBuilder.GetSection("Parameter").GetSection("DbName").Value);
            dict.Add("DbCollectionName", builtBuilder.GetSection("Parameter").GetSection("DbCollectionName").Value); 
            return dict;
        }

        internal static async Task PersistPropertiesUpdateAsync(GameDayRunnerProperties properties)
        {
            var dict = Utils.GetParameters();
            string connectionString = Utils.GetConnectionString();
            MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
            settings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
            var mongoClient = new MongoClient(settings);
            IMongoDatabase mongoDatabase = mongoClient.GetDatabase(dict.GetValueOrDefault("DbName", string.Empty));
            var mongoCollection = mongoDatabase.GetCollection<BsonDocument>(dict.GetValueOrDefault("DbCollectionName", string.Empty));
            var document = properties.ToBsonDocument();
            await mongoCollection.DeleteOneAsync(new BsonDocument());
            await mongoCollection.InsertOneAsync(document);            
        }
    }
}
using System.Collections.Generic;
using System.Security.Authentication;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace AdventureDayRunner.Shared
{
    public class AdventureDayPropertiesRepository
    {
        private readonly AdventureDayDatabaseSettings _settings;
        private readonly IMongoCollection<AdventureDayRunnerProperties> _properties;

        public AdventureDayPropertiesRepository(AdventureDayDatabaseSettings settings)
        {
            _settings = settings;
            var mongoClientSettings = MongoClientSettings.FromUrl(new MongoUrl(settings.ConnectionString));
            mongoClientSettings.SslSettings = new SslSettings() {EnabledSslProtocols = SslProtocols.Tls12};
            
            var client = new MongoClient(mongoClientSettings);
            var database = client.GetDatabase(settings.DatabaseName);

            _properties = database.GetCollection<AdventureDayRunnerProperties>(settings.CollectionName);
        }

        public string Settings => $"Host: {_properties.Database.Client.Settings.Server.Host} | DB: {_settings.DatabaseName} | Collection: {_settings.CollectionName}";

        public List<AdventureDayRunnerProperties> Get() =>
            _properties.Find(properties => true).ToList();

        public AdventureDayRunnerProperties Get(string name) =>
            _properties.Find<AdventureDayRunnerProperties>(properties => properties.Name == name).FirstOrDefault();

        public AdventureDayRunnerProperties Create(AdventureDayRunnerProperties properties)
        {
            _properties.InsertOne(properties);
            return properties;
        }
        public async Task<AdventureDayRunnerProperties> CreateAsync(AdventureDayRunnerProperties properties)
        { 
            await _properties.InsertOneAsync(properties);
            return properties;
        }

        public void Update(string name, AdventureDayRunnerProperties propertiesIn) =>
            _properties.ReplaceOne(properties => properties.Name == name, propertiesIn);
        public Task UpdateAsync(string name, AdventureDayRunnerProperties propertiesIn) =>
            _properties.ReplaceOneAsync(properties => properties.Name == name, propertiesIn);
        public void Remove(AdventureDayRunnerProperties propertiesIn) =>
            _properties.DeleteOne(properties => properties.Name == propertiesIn.Name);
        public Task RemoveAsync(AdventureDayRunnerProperties propertiesIn) =>
            _properties.DeleteOneAsync(properties => properties.Name == propertiesIn.Name);
        public void Remove(string name) =>
            _properties.DeleteOne(properties => properties.Name == name);
        public Task RemoveAsycn(string name) =>
            _properties.DeleteOneAsync(properties => properties.Name == name);
    }
}
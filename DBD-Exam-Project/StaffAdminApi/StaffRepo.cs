using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RestSharp;
using RestSharp.Authenticators;

namespace StaffAdminApi
{
    public class StaffRepo
    {
        private readonly IMongoCollection<Staff> _staffCollection;
        private string apiKey;

        public StaffRepo(IOptions<DatabaseSettings> twitterDatabaseSettings, IConfiguration configuration)
        {
            apiKey = configuration["TWITTER"];

            if (apiKey == null)
                Console.WriteLine("API key for Twitter was null");

            var mongoClient = new MongoClient(
                twitterDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                twitterDatabaseSettings.Value.DatabaseName);

            _staffCollection = mongoDatabase.GetCollection<Staff>(
                twitterDatabaseSettings.Value.CollectionName);
        }


        public async Task<List<Staff>> GetAsync() =>
            await _staffCollection.Find(_ => true).Limit(10).ToListAsync();

        public async Task<Staff?> GetAsync(string id) =>
            await _staffCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Staff newTweet) =>
            await _staffCollection.InsertOneAsync(newTweet);

        public async Task UpdateAsync(string id, Staff updatedTweet) =>
            await _staffCollection.ReplaceOneAsync(x => x.Id == id, updatedTweet);

        public async Task RemoveAsync(string id) =>
            await _staffCollection.DeleteOneAsync(x => x.Id == id);
    }
}
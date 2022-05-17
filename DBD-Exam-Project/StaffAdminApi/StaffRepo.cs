using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RestSharp;
using RestSharp.Authenticators;

namespace StaffAdminApi
{
    public class StaffRepo
    {
        private readonly IMongoCollection<Staff> _staffCollection;

        public StaffRepo(IOptions<DatabaseSettings> twitterDatabaseSettings, IConfiguration configuration)
        {

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

        public async Task CreateAsync(Staff newStaff) =>
            await _staffCollection.InsertOneAsync(newStaff);

        public async Task UpdateAsync(string id, Staff updatedStaff) =>
            await _staffCollection.ReplaceOneAsync(x => x.Id == id, updatedStaff);

        public async Task RemoveAsync(string id) =>
            await _staffCollection.DeleteOneAsync(x => x.Id == id);
    }
}
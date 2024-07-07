using Microsoft.Extensions.Options;
using MongoDB.Driver;
namespace ProtaTestApp.Data
{
    public class MongoDBModel
    {
        private readonly IMongoDatabase _database;

        public MongoDBModel(IOptions<MongoDBSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            _database = client.GetDatabase(settings.Value.DatabaseName);
        }

        public IMongoCollection<T> GetCollection<T>()
        {
            string collectionName = typeof(T).Name;
            return _database.GetCollection<T>(collectionName);
        }
    }
}
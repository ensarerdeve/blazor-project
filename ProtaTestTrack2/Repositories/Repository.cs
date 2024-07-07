using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProtaTestTrack2.Data;

namespace ProtaTestTrack2.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly IMongoCollection<T> _collection;
        
        public Repository(MongoDBModel database)
        {
            _collection = database.GetCollection<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var filter = Builders<T>.Filter.Eq("Discriminator",typeof(T).Name);
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<T> GetByIdAsync(string id)
        {
            var filter = Builders<T>.Filter.Eq("_id", id);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _collection.InsertOneAsync(entity);
        }

        public async Task UpdateAsync(T entity, string id)
        {
            var filter = Builders<T>.Filter.Eq("_id", id);
            await _collection.ReplaceOneAsync(filter, entity);
        }

        public async Task DeleteAsync(string id)
        {
            var filter = Builders<T>.Filter.Eq("_id", id);
            await _collection.DeleteOneAsync(filter);
        }
    }
}

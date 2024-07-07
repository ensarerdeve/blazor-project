using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ProtaTestApp.Data;

namespace ProtaTestApp.Services
{
    public interface IFeatureService
    {
        Task AddFeature(Feature feature);
        Task AddCase(string featureId, Case newCase);
    }
    public class FeatureService : IFeatureService
    {
        private readonly IMongoCollection<Feature> _featureCollection;
        private readonly IMongoCollection<Case> _caseCollection;

        public FeatureService(IOptions<MongoDBSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);

            _featureCollection = database.GetCollection<Feature>("Features");
            _caseCollection = database.GetCollection<Case>("Cases");
        }
        public async Task AddFeature(Feature feature)
        {
            await _featureCollection.InsertOneAsync(feature);
        }
        public async Task AddCase(string featureId, Case newCase)
        {
            newCase.ParentFeatureID = featureId;
            await _caseCollection.InsertOneAsync(newCase);
        }
    }
}

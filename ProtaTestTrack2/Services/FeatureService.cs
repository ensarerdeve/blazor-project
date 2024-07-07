using ProtaTestTrack2.Model;
using ProtaTestTrack2.Repository;

namespace ProtaTestTrack2.Services
{
    public class FeatureService
    {
        private readonly IRepository<Feature> _featureRepository;
        private readonly IRepository<RootFeature> _rootFeatureRepository;

        public FeatureService(IRepository<Feature> featureRepository, IRepository<RootFeature> rootFeatureRepository)
        {
            _featureRepository = featureRepository;
            _rootFeatureRepository = rootFeatureRepository;
        }

        public async Task<IEnumerable<Feature>> GetAllFeaturesAsync()
        {
            return await _featureRepository.GetAllAsync();
        }

        public async Task<Feature> GetFeatureByIdAsync(string id)
        {
            return await _featureRepository.GetByIdAsync(id);
        }

        public async Task<Feature> CreateFeatureAsync(Feature feature)
        {
            feature.FeatureID = Guid.NewGuid().ToString();
            await _featureRepository.AddAsync(feature);

            var rootFeature = await _rootFeatureRepository.GetByIdAsync("RootFeatureID");
            if (rootFeature == null)
            {
                rootFeature = new RootFeature
                {
                    RootID = "RootFeatureID",
                    Features = new List<Feature> { feature }
                };
                await _rootFeatureRepository.AddAsync(rootFeature);
            }
            else
            {
                rootFeature.Features.Add(feature);
                await _rootFeatureRepository.UpdateAsync(rootFeature, "RootFeatureID");
            }

            if (!string.IsNullOrEmpty(feature.ParentFeatureID))
            {
                var parentFeature = await _featureRepository.GetByIdAsync(feature.ParentFeatureID);
                parentFeature.ChildFeatures.Add(feature);
                await _featureRepository.UpdateAsync(parentFeature, feature.ParentFeatureID);
            }
            return feature;
        }

        public async Task DeleteFeatureAsync(string id)
        {
            var featureToDelete = await _featureRepository.GetByIdAsync(id);
            var rootFeature = await _rootFeatureRepository.GetByIdAsync("RootFeatureID");
            if (rootFeature != null)
            {
                var featureToRemove = rootFeature.Features.FirstOrDefault(f => f.FeatureID == id);
                if (featureToRemove != null)
                {
                    rootFeature.Features.Remove(featureToRemove);
                    await _rootFeatureRepository.UpdateAsync(rootFeature, "RootFeatureID");
                }
            }
            if (!string.IsNullOrEmpty(featureToDelete.ParentFeatureID))
            {
                var parentFeature = await _featureRepository.GetByIdAsync(featureToDelete.ParentFeatureID);
                if (parentFeature != null)
                {
                    var featureToRemoveFromParent = parentFeature.ChildFeatures.FirstOrDefault(f => f.FeatureID == id);
                    if (featureToRemoveFromParent != null)
                    {
                        parentFeature.ChildFeatures.Remove(featureToRemoveFromParent);
                        await _featureRepository.UpdateAsync(parentFeature, featureToDelete.ParentFeatureID);
                    }
                }
            }
            await _featureRepository.DeleteAsync(id);
        }

        public async Task UpdateFeatureAsync(Feature updatedFeature)
        {
            await _featureRepository.UpdateAsync(updatedFeature, updatedFeature.FeatureID);
            var rootFeature = await _rootFeatureRepository.GetByIdAsync("RootFeatureID");
            if (rootFeature != null)
            {
                var featureToUpdate = rootFeature.Features.FirstOrDefault(f => f.FeatureID == updatedFeature.FeatureID);
                if (featureToUpdate != null)
                {
                    featureToUpdate.Name = updatedFeature.Name;
                    featureToUpdate.ParentFeatureID = updatedFeature.ParentFeatureID;
                    await _rootFeatureRepository.UpdateAsync(rootFeature, "RootFeatureID");
                }
            }
            if (!string.IsNullOrEmpty(updatedFeature.ParentFeatureID))
            {
                var parentFeature = await _featureRepository.GetByIdAsync(updatedFeature.ParentFeatureID);
                if (parentFeature != null)
                {
                    var childToUpdate = parentFeature.ChildFeatures.FirstOrDefault(cf => cf.FeatureID == updatedFeature.FeatureID);
                    if (childToUpdate != null)
                    {
                        childToUpdate.Name = updatedFeature.Name;
                        childToUpdate.ParentFeatureID = updatedFeature.ParentFeatureID;
                        await _featureRepository.UpdateAsync(parentFeature, updatedFeature.ParentFeatureID);
                    }
                }
            }
        }
    }
}

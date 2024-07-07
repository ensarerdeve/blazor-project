using ProtaTestTrack2.Model;
using ProtaTestTrack2.Repository;

namespace ProtaTestTrack2.Services
{
    public class CaseService
    {
        private readonly IRepository<Case> _caseRepository;
        private readonly FeatureService _featureService;

        public CaseService(IRepository<Case> caseRepository, FeatureService featureService)
        {
            _caseRepository = caseRepository;
            _featureService = featureService;
        }

        public async Task<IEnumerable<Case>> GetAllCasesAsync()
        {
            var features = await _featureService.GetAllFeaturesAsync();
            var allCases = new List<Case>();

            void ExtractCases(IEnumerable<Feature> featuresList)
            {
                foreach (var feature in featuresList)
                {
                    allCases.AddRange(feature.Cases);
                    ExtractCases(feature.ChildFeatures);
                }
            }
            ExtractCases(features);
            return allCases;
        }
        public async Task<Case> GetCaseByIdAsync(string id)
        {
            var features = await _featureService.GetAllFeaturesAsync();
            Case FindCase(IEnumerable<Feature> featuresList)
            {
                foreach (var feature in featuresList)
                {
                    var foundCase = feature.Cases.FirstOrDefault(c => c.CaseID == id);
                    if (foundCase != null)
                    {
                        return foundCase;
                    }
                    var childCase = FindCase(feature.ChildFeatures);
                    if (childCase != null)
                    {
                        return childCase;
                    }
                }
                return null;
            }
            return FindCase(features);
        }

        public async Task<Case> CreateCaseAsync(Case caseItem)
        {
            if (!string.IsNullOrEmpty(caseItem.ParentFeatureID))
            {
                var parentFeature = await _featureService.GetFeatureByIdAsync(caseItem.ParentFeatureID);
                caseItem.CaseID = Guid.NewGuid().ToString();
                parentFeature.Cases.Add(caseItem);
                await _featureService.UpdateFeatureAsync(parentFeature);
                return caseItem;
            }
            else
            {
                throw new ArgumentNullException("ParentFeatureID cannot be null or empty.");
            }
        }

        public async Task UpdateCaseAsync(Case updatedCase)
        {
            await _caseRepository.UpdateAsync(updatedCase, updatedCase.CaseID);
            var parentFeature = await _featureService.GetFeatureByIdAsync(updatedCase.ParentFeatureID);
            if (parentFeature != null)
            {
                var caseToUpdate = parentFeature.Cases.FirstOrDefault(c => c.CaseID == updatedCase.CaseID);
                if (caseToUpdate != null)
                {
                    caseToUpdate.Name = updatedCase.Name;
                    caseToUpdate.ExternalProjectLink = updatedCase.ExternalProjectLink;
                    caseToUpdate.LatestStatus = updatedCase.LatestStatus;
                    caseToUpdate.ExcludedVersions = updatedCase.ExcludedVersions;
                    caseToUpdate.IncluedVersions = updatedCase.IncluedVersions;
                    caseToUpdate.History = updatedCase.History;
                    await _featureService.UpdateFeatureAsync(parentFeature);
                }
            }
        }

        public async Task DeleteCaseAsync(string id)
        {
            var caseToDelete = await GetCaseByIdAsync(id);
            var parentFeature = await _featureService.GetFeatureByIdAsync(caseToDelete.ParentFeatureID);

            if (parentFeature != null)
            {
                var caseToRemove = parentFeature.Cases.FirstOrDefault(c => c.CaseID == id);
                if (caseToRemove != null)
                {
                    parentFeature.Cases.Remove(caseToRemove);
                    await _featureService.UpdateFeatureAsync(parentFeature);
                }
            }
            await _caseRepository.DeleteAsync(id);
        }
    }
}

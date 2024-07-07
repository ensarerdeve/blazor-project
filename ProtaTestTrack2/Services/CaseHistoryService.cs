using ProtaTestTrack2.Model;
using ProtaTestTrack2.Repository;

namespace ProtaTestTrack2.Services
{
    public class CaseHistoryService
    {
        private readonly IRepository<CaseHistory> _caseHistoryRepository;
        private readonly CaseService _caseService;
        private readonly FeatureService _featureService;

        public CaseHistoryService(IRepository<CaseHistory> caseHistoryRepository, CaseService caseService, FeatureService featureService)
        {
            _caseHistoryRepository = caseHistoryRepository;
            _caseService = caseService;
            _featureService = featureService;
        }

        public async Task<IEnumerable<CaseHistory>> GetAllCaseHistoriesAsync()
        {
            var cases = await _caseService.GetAllCasesAsync();
            var caseHistories = cases.SelectMany(c => c.History);
            return caseHistories;
        }

        public async Task<CaseHistory> GetCaseHistoryByIdAsync(string id)
        {
            var cases = await _caseService.GetAllCasesAsync();
            var caseHistory = cases
                .SelectMany(c => c.History)
                .FirstOrDefault(ch => ch.CaseHistoryID == id);

            return caseHistory;
        }

        public async Task<CaseHistory> CreateCaseHistoryAsync(CaseHistory caseHistory)
        {
            if (caseHistory.ParentCaseID != null)
            {
                var parentCase = await _caseService.GetCaseByIdAsync(caseHistory.ParentCaseID);
                caseHistory.CaseHistoryID = Guid.NewGuid().ToString();
                caseHistory.Date = DateTime.UtcNow;
                parentCase.History.Add(caseHistory);
                await _caseService.UpdateCaseAsync(parentCase);
                return caseHistory;
            }
            else
            {
                return null;
            }
        }

        public async Task UpdateCaseHistoryAsync(CaseHistory caseHistory)
        {
            var caseToUpdate = await _caseService.GetCaseByIdAsync(caseHistory.ParentCaseID);
            var existingCaseHistory = caseToUpdate.History.FirstOrDefault(ch => ch.CaseHistoryID == caseHistory.CaseHistoryID);
            if (existingCaseHistory != null)
            {
                existingCaseHistory.ParentCaseID = caseHistory.ParentCaseID;
                existingCaseHistory.Date = DateTime.UtcNow;
                existingCaseHistory.Notes = caseHistory.Notes;
                existingCaseHistory.Tester = caseHistory.Tester;
                existingCaseHistory.JiraNumber = caseHistory.JiraNumber;
                existingCaseHistory.Status = caseHistory.Status;
                await _caseService.UpdateCaseAsync(caseToUpdate);
            }
        }

        public async Task DeleteCaseHistoryAsync(string id)
        {
            var cases = await _caseService.GetAllCasesAsync();
            var caseToUpdate = cases.FirstOrDefault(c => c.History.Any(ch => ch.CaseHistoryID == id));
            var caseHistoryToDelete = caseToUpdate.History.FirstOrDefault(ch => ch.CaseHistoryID == id);
            if (caseHistoryToDelete != null)
            {
                caseToUpdate.History.Remove(caseHistoryToDelete);
                await _caseService.UpdateCaseAsync(caseToUpdate);
                var feature = await _featureService.GetFeatureByIdAsync(caseToUpdate.ParentFeatureID);
                if (feature != null)
                {
                    var caseInFeature = feature.Cases.FirstOrDefault(c => c.CaseID == caseToUpdate.CaseID);
                    if (caseInFeature != null)
                    {
                        caseInFeature.History = caseToUpdate.History;
                        await _featureService.UpdateFeatureAsync(feature);
                    }
                }
            }
        }
    }
}

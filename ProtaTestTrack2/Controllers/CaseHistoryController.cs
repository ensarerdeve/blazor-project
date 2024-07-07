using Microsoft.AspNetCore.Mvc;
using ProtaTestTrack2.Model;
using ProtaTestTrack2.Services;

namespace ProtaTestTrack2.Controllers
{
    [Route("CaseHistory")]
    [ApiController]
    public class CaseHistoryController : ControllerBase
    {
        private readonly CaseHistoryService _caseHistoryService;
        public CaseHistoryController(CaseHistoryService caseHistoryService)
        {
            _caseHistoryService = caseHistoryService;
        }
        [HttpGet("GetAllCaseHistory")]
        public async Task<ActionResult<IEnumerable<CaseHistory>>> GetCaseHistory()
        {
            var caseHistories = await _caseHistoryService.GetAllCaseHistoriesAsync();
            return Ok(caseHistories);
        }
        [HttpGet("GetCaseHistoryById/{id}")]
        public async Task<ActionResult<CaseHistory>> GetCaseHistory(string id)
        {
            var caseHistory = await _caseHistoryService.GetCaseHistoryByIdAsync(id);
            if (caseHistory == null)
            {
                return NotFound();
            }
            return Ok(caseHistory);
        }
        [HttpPost("CreateCaseHistory")]
        public async Task<ActionResult<CaseHistory>> PostCaseHistory(CaseHistory caseHistory)
        {
            try
            {
                await _caseHistoryService.CreateCaseHistoryAsync (caseHistory);
                return CreatedAtAction(nameof(GetCaseHistory), new { id = caseHistory.ParentCaseID }, caseHistory);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpPut("UpdateCaseHistory")]
        public async Task<IActionResult> UpdateCaseHistory(CaseHistory caseHistory)
        {
            if (null == caseHistory.ParentCaseID)
            {
                return BadRequest();
            }
            try
            {
                await _caseHistoryService.UpdateCaseHistoryAsync(caseHistory);
                return Ok(caseHistory);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpDelete("DeleteCaseHistory/{id}")]
        public async Task<IActionResult> DeleteCaseHistory(string id)
        {
            try
            {
                await _caseHistoryService.DeleteCaseHistoryAsync(id);
                return NoContent();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}

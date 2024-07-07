using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ProtaTestApp.Data;
using ProtaTestApp.Services;

namespace ProtaTestApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeatureController : ControllerBase
    {
        private readonly IFeatureService _featureService;

        public FeatureController(IFeatureService featureService)
        {
            _featureService = featureService;
        }

        [HttpPost("addFeature")]
        public async Task<IActionResult> AddFeature([FromBody] Feature feature)
        {
            try
            {
                await _featureService.AddFeature(feature);
                return Ok("Feature added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("addCase/{featureId}")]
        public async Task<IActionResult> AddCase(string featureId, [FromBody] Case newCase)
        {
            try
            {
                await _featureService.AddCase(featureId, newCase);
                return Ok("Case added successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using ProtaTestTrack2.Services;
using ProtaTestTrack2.Model;


namespace ProtaTestTrack2.Controllers
{
    [ApiController]
    [Route("Feature")]
    public class FeatureController : ControllerBase
    {
        private readonly FeatureService _featureService;

        public FeatureController(FeatureService featureService)
        {
            _featureService = featureService;
        }

        [HttpGet("GetAllFeatures")]
        public async Task<ActionResult<IEnumerable<Feature>>> GetAllFeatures()
        {
            try
            {
                var features = await _featureService.GetAllFeaturesAsync();
                return Ok(features);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpGet("GetFeatureById/{id}")]
        public async Task<ActionResult<Feature>> GetFeature(string id)
        {
            try
            {
                var feature = await _featureService.GetFeatureByIdAsync(id);
                if (feature == null)
                {
                    return NotFound();
                }
                return Ok(feature);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpPost("CreateFeature")]
        public async Task<ActionResult<Feature>> CreateFeature(Feature feature)
        {
            try
            {
                var createdFeature = await _featureService.CreateFeatureAsync(feature);
                return CreatedAtAction(nameof(GetFeature), new { id = createdFeature.FeatureID }, createdFeature);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
        [HttpDelete("DeleteFeature/{id}")]
        public async Task<IActionResult> DeleteFeature(string id)
        {
            try
            {
                await _featureService.DeleteFeatureAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
        [HttpPut("UpdateFeature")]
        public async Task<IActionResult> UpdateFeature(Feature updatedFeature)
        {
            try
            {
                await _featureService.UpdateFeatureAsync(updatedFeature);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }
}
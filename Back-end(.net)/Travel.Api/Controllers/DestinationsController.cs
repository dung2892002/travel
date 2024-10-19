using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Travel.Core.Interfaces.IServices;

namespace Travel.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DestinationsController(IDestinationService destinationService) : ControllerBase
    {
        private readonly IDestinationService _destinationService = destinationService;

        [HttpGet("{cityId}")]
        public async Task<IActionResult> GetDestinationOfCity(int cityId)
        {
            try
            {
                var destiantions = await _destinationService.GetByCity(cityId);
                return StatusCode(201, destiantions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(Policy = "Admin")]
        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadImages([FromForm] List<IFormFile> files, [FromForm] int destinationId)
        {
            try
            {

                var result = await _destinationService.UploadImagesAsync(files, destinationId);
                if (!result) return StatusCode(400, "Failed to upload images.");
                return StatusCode(201, "upload image successfully");
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}

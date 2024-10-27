using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Travel.Core.Entities;
using Travel.Core.Interfaces.IServices;

namespace Travel.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ToursController(ITourService tourService) : ControllerBase
    {
        private readonly ITourService _tourService = tourService;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var hotels = await _tourService.GetAll();
                return StatusCode(200, hotels);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("destination/{id}")]
        public async Task<IActionResult> GetByDestination(int id)
        {
            try
            {
                var hotels = await _tourService.GetByDestination(id);
                return StatusCode(200, hotels);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(Policy = "TourPartner")]
        [HttpGet("partner/{id}")]
        public async Task<IActionResult> GetByPartner(Guid id)
        {
            try
            {
                var hotels = await _tourService.GetByPartner(id);
                return StatusCode(200, hotels);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(Policy = "TourPartner")]
        [HttpPost("edit/upload-image")]
        public async Task<IActionResult> UploadImages([FromForm] List<IFormFile> files, [FromQuery] Guid tourId)
        {
            try
            {

                var result = await _tourService.UploadImagesAsync(files, tourId);
                if (!result) return StatusCode(400, "Failed to upload images.");
                return StatusCode(201, "upload image successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(Policy = "TourPartner")]
        [HttpPost]
        public async Task<IActionResult> CreateTour([FromBody] Tour tour)
        {
            try
            {
                await _tourService.Create(tour);
                return StatusCode(200, "create tour successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}

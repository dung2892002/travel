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
                var tour = await _tourService.GetAll();
                return StatusCode(200, tour);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("detail")]
        public async Task<IActionResult> GetDetail([FromQuery] Guid id)
        {
            try
            {
                var tour = await _tourService.GetDetail(id);
                return StatusCode(200, tour);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("city")]
        public async Task<IActionResult> GetByCity([FromQuery] int id)
        {
            try
            {
                var tour = await _tourService.GetByCity(id);
                return StatusCode(200, tour);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(Policy = "TourPartner")]
        [HttpGet("partner")]
        public async Task<IActionResult> GetByPartner([FromQuery] Guid id)
        {
            try
            {
                var tour = await _tourService.GetByPartner(id);
                return StatusCode(200, tour);

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
                return StatusCode(201, "create tour successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(Policy = "TourPartner")]
        [HttpPut("edit")]
        public async Task<IActionResult> UpdateTour([FromBody] Tour tour, [FromQuery] Guid tourId)
        {
            try
            {
                await _tourService.Update(tour, tourId);
                return StatusCode(200, "update tour successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(Policy = "TourPartner")]
        [HttpPost("schedule")]
        public async Task<IActionResult> CreateTourSchedule([FromBody] TourSchedule tourSchedule)
        {
            try
            {
                await _tourService.CreateSchedule(tourSchedule);
                return StatusCode(201, "create schedule successfully");
            }
            catch (ArgumentException ex)
            {
                return StatusCode(400, ex.Message);
            }

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(Policy = "TourPartner")]
        [HttpGet("schedule")]
        public async Task<IActionResult> GetSchedule([FromQuery] Guid tourId)
        {
            try
            {
                var schedules = await _tourService.GetScheduleByTour(tourId);
                return StatusCode(200, schedules);
            }
            catch (ArgumentException ex)
            {
                return StatusCode(400, ex.Message);
            }

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(Policy = "TourPartner")]
        [HttpGet("schedule/available")]
        public async Task<IActionResult> GetScheduleAvailable([FromQuery] Guid tourId)
        {
            try
            {
                var schedules = await _tourService.GetScheduleAvailableByTour(tourId);
                return StatusCode(200, schedules);
            }
            catch (ArgumentException ex)
            {
                return StatusCode(400, ex.Message);
            }

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}

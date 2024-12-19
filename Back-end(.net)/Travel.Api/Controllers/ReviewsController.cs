using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Travel.Core.Entities;
using Travel.Core.Interfaces.IServices;

namespace Travel.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ReviewsController(IReviewService reviewService) : ControllerBase
    {
        private readonly IReviewService _reviewService = reviewService;

        [HttpGet("hotel")]
        public async Task<IActionResult> GetByHotel([FromQuery] Guid id, [FromQuery] int pageNumber)
        {
            try
            {
                var reviews = await _reviewService.GetByHotel(id, pageNumber);
                return StatusCode(200, reviews);
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

        [HttpGet("tour")]
        public async Task<IActionResult> GetByTour([FromQuery] Guid id, [FromQuery] int pageNumber)
        {
            try
            {
                var reviews = await _reviewService.GetByTour(id, pageNumber);
                return StatusCode(200, reviews);
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

        [HttpGet("destination")]
        public async Task<IActionResult> GetByDestination([FromQuery] int id)
        {
            try
            {
                var reviews = await _reviewService.GetByDestination(id);
                return StatusCode(200, reviews);
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

        [Authorize(Policy = "User")]
        [HttpPost]
        public async Task<IActionResult> CreateReview([FromForm] Review review, [FromForm] List<IFormFile> files)
        {
            try
            {
                await _reviewService.Create(review, files);
                return StatusCode(201, "create review successfully");
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

        [Authorize(Policy = "User")]
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteReview([FromQuery] int id, [FromBody] Guid userId)
        {
            try
            {
                var result = await _reviewService.Delete(id, userId);
                if (result)
                {
                    return StatusCode(200, "delete review successfully");
                }
                return StatusCode(404, "not found");
            }
            catch (UnauthorizedAccessException ex) {
                return StatusCode(403, ex.Message);
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

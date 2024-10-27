using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Travel.Core.Entities;
using Travel.Core.Interfaces.IServices;

namespace Travel.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class HotelsController(IHotelService hotelService) : ControllerBase
    {
        private readonly IHotelService _hotelService = hotelService;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var hotels = await _hotelService.GetAll();
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
                var hotels = await _hotelService.GetByDestination(id);
                return StatusCode(200, hotels);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(Policy = "HotelPartner")]
        [HttpGet("partner/{id}")]
        public async Task<IActionResult> GetByPartner(Guid id)
        {
            try
            {
                var hotels = await _hotelService.GetByPartner(id);
                return StatusCode(200, hotels);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(Policy = "HotelPartner")]
        [HttpPost]
        public async Task<IActionResult> CreateHotel([FromBody] Hotel hotel)
        {
            try
            {
                await _hotelService.CreateHotel(hotel);
                return StatusCode(201, "create hotel successfully");

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(Policy = "HotelPartner")]
        [HttpPut("edit")]
        public async Task<IActionResult> UpdateHotel([FromQuery] Guid hotelId, [FromBody] Hotel hotel)
        {
            try
            {
                var result = await _hotelService.UpdateHotel(hotelId, hotel);
                if (!result)
                {
                    return StatusCode(400, "update fail");
                }
                return StatusCode(200, "update hotel successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(Policy = "HotelPartner")]
        [HttpPost("edit/upload-image")]
        public async Task<IActionResult> UploadImages([FromForm] List<IFormFile> files,[FromQuery] Guid hotelId)
        {
            try
            {

                var result = await _hotelService.UploadImagesAsync(files, hotelId);
                if (!result) return StatusCode(400, "Failed to upload images.");
                return StatusCode(201, "upload image successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(Policy = "HotelPartner")]
        [HttpPost("edit/add-destination")]
        public async Task<IActionResult> AddDestination([FromQuery] Guid hotelId, [FromBody] List<Destination> destinations)
        {
            try
            {
                var result = await _hotelService.AddDestination(destinations, hotelId);
                if (!result) return StatusCode(400, "Failed to upload images.");
                return StatusCode(201, "add destination successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}

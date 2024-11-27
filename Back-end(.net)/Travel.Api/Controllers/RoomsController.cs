using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Travel.Core.DTOs;
using Travel.Core.Entities;
using Travel.Core.Interfaces.IServices;
using Travel.Core.Services;

namespace Travel.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RoomsController(IRoomService roomService) : ControllerBase
    {
        private readonly IRoomService _roomService = roomService;

        [HttpGet]
        public async Task<IActionResult> GetByHotel([FromQuery] Guid hotelId)
        {
            try
            {
                var rooms = await _roomService.GetRoomByHotel(hotelId);
                return StatusCode(200, rooms);
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var room = await _roomService.GetRoomDetail(id);
                return StatusCode(200, room);
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

        [HttpPost("search")]
        public async Task<IActionResult> SearchRoom([FromBody] SearchRoomRequest request)
        {
            try
            {
                var response = await _roomService.SearchRoom(request);
                return StatusCode(200, response);
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

        [Authorize(Policy = "HotelPartner")]
        [HttpPost]
        public async Task<IActionResult> CreateRoom([FromBody] Room room)
        {
            try
            {
                await _roomService.CreateRoom(room);
                return StatusCode(201, "create room successfully");
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

        [Authorize(Policy = "HotelPartner")]
        [HttpPut("edit")]
        public async Task<IActionResult> UpdateRoom([FromQuery]Guid roomId, [FromBody] Room room)
        {
            try
            {
                var result = await _roomService.UpdateRoom(roomId, room);
                if (!result) return StatusCode(400, "fail to upadte");
                return StatusCode(200, "update room successfully");
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

        [Authorize(Policy = "HotelPartner")]
        [HttpPost("edit/upload-image")]
        public async Task<IActionResult> UploadRoomImage([FromQuery] Guid roomId, [FromForm] List<IFormFile> files)
        {
            try
            {
                await _roomService.UploadImagesAsync(files, roomId);
                return StatusCode(201, "upload image successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}

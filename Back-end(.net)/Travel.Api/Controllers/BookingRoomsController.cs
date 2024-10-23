using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Travel.Core.Entities;
using Travel.Core.Interfaces.IServices;

namespace Travel.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BookingRoomsController(IBookingRoomService bookingRoomService) : ControllerBase
    {
        private readonly IBookingRoomService _bookingRoomService = bookingRoomService;


        [Authorize(Policy = "User")]
        [HttpGet("user")]
        public async Task<IActionResult> GetByUser([FromQuery] Guid id)
        {
            try
            {
                var bookings = await _bookingRoomService.GetByUser(id);
                return StatusCode(200, bookings);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("hotel")]
        public async Task<IActionResult> GetByHotel([FromQuery] int id)
        {
            try
            {
                var bookings = await _bookingRoomService.GetByHotel(id);
                return StatusCode(200, bookings);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("room")]
        public async Task<IActionResult> GetByRoom([FromQuery] int id)
        {
            try
            {
                var bookings = await _bookingRoomService.GetByRoom(id);
                return StatusCode(200, bookings);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Policy = "User")]
        [HttpPost]
        public async Task<IActionResult> CreateBookingRoom([FromBody] BookingRoom booking)
        {
            try
            {
                await _bookingRoomService.Create(booking);
                return StatusCode(200, "creat booking room successfull");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

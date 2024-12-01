using Google.Apis.Storage.v1.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Travel.Core.Entities;
using Travel.Core.Interfaces.IServices;

namespace Travel.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BookingsRoomController(IBookingRoomService bookingRoomService) : ControllerBase
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
                return StatusCode(400,ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }

        [HttpGet("hotel")]
        public async Task<IActionResult> GetByHotel([FromQuery] Guid id)
        {
            try
            {
                var bookings = await _bookingRoomService.GetByHotel(id);
                return StatusCode(200, bookings);
            }
            catch (ArgumentException ex)
            {
                return StatusCode(400,ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("room")]
        public async Task<IActionResult> GetByRoom([FromQuery] Guid id)
        {
            try
            {
                var bookings = await _bookingRoomService.GetByRoom(id);
                return StatusCode(200, bookings);
            }
            catch (ArgumentException ex)
            {
                return StatusCode(400,ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }

        [Authorize(Policy = "User")]
        [HttpPost]
        public async Task<IActionResult> CreateBookingRoom([FromBody] BookingRoom booking)
        {
            try
            {
                await _bookingRoomService.Create(booking);
                return StatusCode(201, "creat booking room successfull");
            }
            catch (ArgumentException ex)
            {
                return StatusCode(400, ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(409, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [Authorize(Policy = "User")]
        [HttpPatch("cancel")]
        public async Task<IActionResult> CancelBooking([FromQuery] Guid id, [FromBody] string reason)
        {
            try
            {
                await _bookingRoomService.CancelBooking(id, reason);
                return StatusCode(200, "cancel room successfull");
            }
            catch (ArgumentException ex)
            {
                return StatusCode(400, ex.Message);
            }
            catch (InvalidCastException ex)
            {
                return StatusCode(409, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}

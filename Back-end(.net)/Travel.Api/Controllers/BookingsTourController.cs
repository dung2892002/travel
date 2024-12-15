using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Travel.Core.Entities;
using Travel.Core.Interfaces.IServices;
using Travel.Core.Services;

namespace Travel.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BookingsTourController(IBookingTourService bookingTourService) : ControllerBase
    {
        private readonly IBookingTourService _bookingTourService = bookingTourService;

        [Authorize(Policy = "User")]
        [HttpGet("user")]
        public async Task<IActionResult> GetByUser([FromQuery] Guid id, [FromQuery] int? status, [FromQuery] int pageNumber)
        {
            try
            {
                var bookings = await _bookingTourService.GetByUser(id, status, pageNumber);
                return StatusCode(200, bookings);
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
        public async Task<IActionResult> GetByTour([FromQuery] Guid id)
        {
            try
            {
                var bookings = await _bookingTourService.GetByTour(id);
                return StatusCode(200, bookings);
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
        public async Task<IActionResult> CreateBookingTour([FromBody] BookingTour booking)
        {
            try
            {
                await _bookingTourService.Create(booking);
                return StatusCode(201, "creat booking tour successfull");
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
        [HttpPatch("cancel")]
        public async Task<IActionResult> CancelBooking([FromQuery] Guid id, [FromBody] string reason)
        {
            try
            {
                await _bookingTourService.CancelBooking(id, reason);
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

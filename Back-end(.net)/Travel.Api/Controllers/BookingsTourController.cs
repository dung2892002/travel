using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Travel.Core.Entities;
using Travel.Core.Interfaces.IServices;

namespace Travel.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BookingsTourController(IBookingTourService bookingTourService) : ControllerBase
    {
        private readonly IBookingTourService _bookingTourService = bookingTourService;

        [Authorize(Policy = "User")]
        [HttpGet("user")]
        public async Task<IActionResult> GetByUser([FromQuery] Guid id)
        {
            try
            {
                var bookings = await _bookingTourService.GetByUser(id);
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
                return StatusCode(200, "creat booking tour successfull");
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

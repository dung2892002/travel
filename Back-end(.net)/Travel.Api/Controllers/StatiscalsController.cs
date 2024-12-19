using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Travel.Core.DTOs;
using Travel.Core.Interfaces.IServices;

namespace Travel.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class StatiscalsController(IStatiscalService statiscalService) : ControllerBase
    {
        private readonly IStatiscalService _statiscalService = statiscalService;

        [Authorize(Policy = "Admin")]
        [HttpGet("admin")]
        public async Task<IActionResult> AdminStatiscal([FromQuery] DateTime start, [FromQuery] DateTime end, [FromQuery] Guid? hotelId, [FromQuery] Guid? tourId)
        {
            try
            {
                var request = new StatisticalRequest
                {
                    Start = start,
                    End = end,
                    HotelId = hotelId,
                    TourId = tourId
                };

                var result = await _statiscalService.AdminStatiscal(request);
                return StatusCode(200, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(Policy = "HotelPartner")]
        [HttpGet("hotel")]
        public async Task<IActionResult> HotelStatiscal([FromQuery] DateTime start, [FromQuery] DateTime end, [FromQuery] Guid userId, [FromQuery] Guid? hotelId)
        {
            try
            {
                var request = new StatisticalRequest
                {
                    Start = start,
                    End = end,
                    HotelId = hotelId,
                    UserId = userId
                };

                var result = await _statiscalService.HotelStatiscal(request);
                return StatusCode(200, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(Policy = "TourPartner")]
        [HttpGet("tour")]
        public async Task<IActionResult> TourStatiscal([FromQuery] DateTime start, [FromQuery] DateTime end, [FromQuery] Guid userId, [FromQuery] Guid? tourId)
        {
            try
            {
                var request = new StatisticalRequest
                {
                    Start = start,
                    End = end,
                    TourId = tourId,
                    UserId = userId
                };

                var result = await _statiscalService.TourStatiscal(request);
                return StatusCode(200, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Travel.Core.Entities;
using Travel.Core.Interfaces.IServices;

namespace Travel.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DiscountsController(IDiscountService discountService) : ControllerBase
    {
        private readonly IDiscountService _discountService = discountService;


        [HttpGet("user")]
        public async Task<IActionResult> GetDiscountByUser([FromQuery] Guid userId)
        {
            try
            {
                var discounts = await _discountService.GetByUser(userId);
                return StatusCode(200, discounts);
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

        [HttpGet("hotel")]
        public async Task<IActionResult> GetDiscountByHotel([FromQuery] Guid hotelId, [FromQuery] decimal price)
        {
            try
            {
               var discounts = await _discountService.GetByHotel(hotelId, price);
               return StatusCode(200, discounts);
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
        public async Task<IActionResult> GetDiscountByTour([FromQuery] Guid tourId, [FromQuery] decimal price)
        {
            try
            {
                var discounts = await _discountService.GetByTour(tourId, price);
                return StatusCode(200, discounts);
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

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> AdminCreateDiscount([FromBody] Discount discount)
        {
            try
            {
                discount.Type = 0;
                discount.DiscountHotel = [];
                discount.DiscountTour = [];

                await _discountService.AdminCreate(discount);
                return StatusCode(201, "admin create discount successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("hotel")]
        [Authorize(Policy = "HotelPartner")]
        public async Task<IActionResult> CreateHotelDiscount([FromBody] Discount discount)
        {
            try
            {
                discount.Type = 1;

                foreach (var discountHotel in discount.DiscountHotel) {
                    Console.WriteLine($"khach san them khuyen mai co id la:{discountHotel.HotelId}");
                }
                await _discountService.CreateDiscountHotel(discount);
                return StatusCode(201, "create discount hotel successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("tour")]
        [Authorize(Policy = "TourPartner")]
        public async Task<IActionResult> CreateTourDiscount([FromBody] Discount discount)
        {
            try
            {
                discount.Type = 1;
                discount.DiscountHotel = [];

                await _discountService.CreateDiscountTour(discount);
                return StatusCode(201, "create discount tour successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}

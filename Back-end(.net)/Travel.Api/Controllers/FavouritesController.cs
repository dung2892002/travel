using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Travel.Core.Entities;
using Travel.Core.Interfaces.IServices;

namespace Travel.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class FavouritesController(IFavouriteService favouriteService) : ControllerBase
    {
        private readonly IFavouriteService _favouriteService = favouriteService;

        [Authorize(Policy = "User")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Favourite favourite)
        {
            try
            {
                await _favouriteService.Create(favourite);
                return StatusCode(201, "create favourite successfully");
            }
            catch
            (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize(Policy = "User")]
        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            try
            {
                var result = await _favouriteService.Delete(id);
                if (result)
                {
                    return StatusCode(200, "delete favourite successfully");
                }
                return StatusCode(404, "not found");
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

        [HttpGet("user")]
        public async Task<IActionResult> GetByUser([FromQuery] Guid id)
        {
            try
            {
                var favourites = await _favouriteService.GetByUser(id);
                return StatusCode(200, favourites);
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
        public async Task<IActionResult> GetByHotel([FromQuery] Guid id)
        {
            try
            {
                var favourites = await _favouriteService.GetQuantityByHotel(id);
                return StatusCode(200, favourites);
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
        [HttpGet("hotel/check")]
        public async Task<IActionResult> GetUserFavouriteHotel([FromQuery] Guid userId, [FromQuery] Guid hotelId)
        {
            try
            {
                var check = await _favouriteService.GetUserFavouriteHotel(userId, hotelId);
                Console.WriteLine("user co thich hotel khoong: ", check);
                return StatusCode(200, check);
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
                var favourite = await _favouriteService.GetQuantityByTour(id);
                return StatusCode(200, favourite);
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
        [HttpGet("tour/check")]
        public async Task<IActionResult> GetUserFavouriteTour([FromQuery] Guid userId, [FromQuery] Guid tourId)
        {
            try
            {
                var favourite = await _favouriteService.GetUserFavouriteTour(userId, tourId);
                return StatusCode(200, favourite);
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

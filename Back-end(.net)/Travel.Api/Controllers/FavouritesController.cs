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
        
    }
}

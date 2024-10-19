using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Travel.Core.Interfaces.IServices;

namespace Travel.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CitiesController(ICityService cityService) : ControllerBase
    {
        private readonly ICityService _cityService = cityService;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var cities = await _cityService.GetAll();
                return StatusCode(200, cities);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet("{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            try
            {
                var cities = await _cityService.GetByName(name);
                return StatusCode(200, cities);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("province/{id}")]
        public async Task<IActionResult> GetByProvinceId(int id)
        {
            try
            {
                var cities = await _cityService.GetByProvinceId(id);
                return StatusCode(200, cities);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}

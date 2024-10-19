using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Travel.Core.Interfaces.IServices;

namespace Travel.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProvincesController(IProvinceService provinceService) : ControllerBase
    {
        private readonly IProvinceService _provinceService = provinceService;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var provinces = await _provinceService.GetAll();
                return StatusCode(200, provinces);
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
                var provinces = await _provinceService.GetByName(name);
                return StatusCode(200, provinces);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}

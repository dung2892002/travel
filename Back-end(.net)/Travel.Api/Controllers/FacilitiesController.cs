using Microsoft.AspNetCore.Mvc;
using Travel.Core.Interfaces.IServices;

namespace Travel.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class FacilitiesController(IFacilityService facilityService) : ControllerBase
    {
        private readonly IFacilityService _facilityService = facilityService;

        [HttpGet]
        public async Task<IActionResult> GetFacilities([FromQuery]int type)
        {
            try
            {
                var facilities = await _facilityService.GetFacilities(type);
                return StatusCode(200, facilities);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}

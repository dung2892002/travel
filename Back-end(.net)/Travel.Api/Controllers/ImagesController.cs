using Microsoft.AspNetCore.Mvc;
using Travel.Core.Interfaces;

namespace Travel.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ImagesController(IFirebaseStorageService firebaseStorageService) : ControllerBase
    {
        private readonly  IFirebaseStorageService _firebaseStorageService = firebaseStorageService;

        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null)
            {
                return BadRequest("No file upload");
            }

            using Stream stream = file.OpenReadStream();
            var fileName = file.FileName;
            var url = await _firebaseStorageService.UploadFileAsync(stream, fileName);
            return Ok(url);
        }
    }
}

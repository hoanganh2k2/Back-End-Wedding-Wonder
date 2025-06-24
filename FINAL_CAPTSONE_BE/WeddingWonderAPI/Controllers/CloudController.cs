using BusinessObject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using WeddingWonderAPI.Helper;
using WeddingWonderAPI.Models.Response;
using System.Threading.Tasks;
using Repository.IRepositories;
using System.Security.Claims;

namespace WeddingWonderAPI.Controllers
{
    [Route("api/cloud")]
    [ApiController]
    public class CloudController : ControllerBase
    {
        private readonly ICloudStorageRepository _cloudStorageService;

        public CloudController(ICloudStorageRepository cloudStorageService)
        {
            _cloudStorageService = cloudStorageService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            int userId = UserHelper.GetUserId(User);

            if (userId <= 0)
                return BadRequest("User ID is required.");

            try
            {
                using (var stream = file.OpenReadStream())
                {
                    var fileUrl = await _cloudStorageService.UploadFileAsync(userId.ToString(), stream, file.FileName, file.ContentType);
                    return Ok(new { FileUrl = fileUrl });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}

using BusinessObject.DTOs;
using Microsoft.AspNetCore.Mvc;
using Services;
using WeddingWonderAPI.Models.Response;

namespace WeddingWonderAPI.Controllers
{
    [Route("api/admin")]
    [ApiController]
    public class InFoAdminController : ControllerBase
    {
        private readonly InForAdminService _inFoAdminServiceManager;

        public InFoAdminController(InForAdminService inFoAdminServiceManager)
        {
            _inFoAdminServiceManager = inFoAdminServiceManager;
        }

        [HttpGet("all")]

        public async Task<ActionResult> GetInFoAdmins()
        {
            try
            {
                List<InForAdminDTO> admins = await _inFoAdminServiceManager.GetAllInForAdminsAsync();
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Retrieved admin information successfully",
                    Data = admins
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse
                {
                    Status = false,
                    Message = $"{ex.Message}"
                });
            }
        }
    }
}

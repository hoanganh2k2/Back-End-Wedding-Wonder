using BusinessObject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using WeddingWonderAPI.Helper;
using WeddingWonderAPI.Models.Response;
using System.Threading.Tasks;

namespace WeddingWonderAPI.Controllers
{
    [Route("api/admin-log")]
    [ApiController]
    public class AdminLogController : ControllerBase
    {
        private readonly AdminLogService _adminLogService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AdminLogController(AdminLogService adminLogService, IHttpContextAccessor httpContextAccessor)
        {
            _adminLogService = adminLogService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("allLogs")]
        [Authorize(Roles = "Super Admin")]
        public async Task<ActionResult> GetAllLogs()
        {
            try
            {
                var logs = await _adminLogService.GetAllLogsAsync();
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Fetched all admin logs successfully",
                    Data = logs
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Status = false,
                    Message = ex.Message
                });
            }
        }
         
        [HttpGet("{logId}")]
        [Authorize(Roles = "Super Admin")]
        public async Task<ActionResult> GetLogById(int logId)
        {
            try
            {
                var log = await _adminLogService.GetLogByIdAsync(logId);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Fetched admin log by ID",
                    Data = log
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Status = false,
                    Message = ex.Message
                });
            }
        }
         
        [HttpGet("by-admin/{adminId}")]
        [Authorize(Roles = "Super Admin")]
        public async Task<ActionResult> GetLogsByAdminId(int adminId)
        {
            try
            {
                var logs = await _adminLogService.GetLogsByAdminIdAsync(adminId);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Fetched logs by Admin ID",
                    Data = logs
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Status = false,
                    Message = ex.Message
                });
            }
        }
         
        [HttpPost("create")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CreateLog([FromBody] AdminLog log)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse
                {
                    Status = false,
                    Message = "Invalid data"
                });
            }

            try
            {
                await _adminLogService.CreateLogAsync(log);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Log created successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Status = false,
                    Message = ex.Message
                });
            }
        }  
    }
}

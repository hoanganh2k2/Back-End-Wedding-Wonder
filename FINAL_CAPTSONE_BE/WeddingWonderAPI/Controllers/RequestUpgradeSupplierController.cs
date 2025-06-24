using BusinessObject.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;
using WeddingWonderAPI.Helper;

namespace WeddingWonderAPI.Controllers
{
    [Route("api/upgrade")]
    [ApiController]
    public class RequestUpgradeSupplierController : ControllerBase
    {
        private readonly RequestUpgradeSupplierService _requestService;

        public RequestUpgradeSupplierController(RequestUpgradeSupplierService requestService)
        {
            _requestService = requestService;
        }

        [HttpGet("allrequest")]
        [Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> GetPendingRequests()
        {
            try
            {
                var pendingRequests = await _requestService.GetPendingRequestsAsync();
                return Ok(new { Success = true, Data = pendingRequests });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, Error = ex.Message });
            }
        }


        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CreateRequest([FromBody] CreateRequestDTO createRequestDto)
        {
            try
            {
                var isCreated = await _requestService.CreateRequestAsync(
                    createRequestDto.RequestDto,
                    createRequestDto.ImageDtos
        );
                return Ok(new { Success = isCreated });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPut("{requestId}/accept")]
        [Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> AcceptRequest(int requestId)
        {
            try
            {
                var isAccepted = await _requestService.AcceptRequestAsync(requestId);
                return Ok(new { Success = isAccepted });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPut("{requestId}/reject")]
        [Authorize(Roles = "Super Admin")]
        public async Task<IActionResult> RejectRequest(int requestId, [FromBody] string rejectReason)
        {
            try
            {
                var isRejected = await _requestService.RejectRequestAsync(requestId, rejectReason);
                return Ok(new { Success = isRejected });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
        [HttpPut("confirm-upgrade")]
        [Authorize(Roles = "Customer, Supplier")]
        public async Task<IActionResult> ConfirmUpgrade()
        {
            try
            {
                int userId = UserHelper.GetUserId(User);
                var isConfirmed = await _requestService.ConfirmUpgradeAsync(userId);
                return Ok(new { Success = isConfirmed, Message = "Xác nhận nâng cấp thành công." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, Error = ex.Message });
            }
        }
        [HttpGet("approved-business-types")]
        [Authorize(Roles = "Supplier")]
        public async Task<IActionResult> GetApprovedBusinessTypes()
        {
            try
            {
                var userId = UserHelper.GetUserId(User);
                var businessTypes = await _requestService.GetApprovedBusinessTypesAsync(userId);

                return Ok(new { Success = true, Data = businessTypes });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, Error = ex.Message });
            }
        }
    }
}

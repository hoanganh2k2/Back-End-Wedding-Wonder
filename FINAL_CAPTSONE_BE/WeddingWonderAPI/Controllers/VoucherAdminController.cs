using BusinessObject.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using WeddingWonderAPI.Models.Response;

namespace WeddingWonderAPI.Controllers
{
    [Route("api/voucher")]
    [ApiController]
    public class VoucherAdminController : ControllerBase
    {
        private readonly VoucherAdminService _voucherAdminServiceManager;

        public VoucherAdminController(VoucherAdminService voucherAdminServiceManager)
        {
            _voucherAdminServiceManager = voucherAdminServiceManager;
        }

        [HttpGet("all")]
        [Authorize(Roles = "Admin, Super Admin")]
        public async Task<ActionResult> GetVoucherAdmins()
        {
            try
            {
                List<VoucherAdminDTO> vouchers = await _voucherAdminServiceManager.GetAllVoucherAdminsAsync();
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Retrieved voucher information successfully",
                    Data = vouchers
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

        [HttpGet("upcoming")]
        [Authorize(Roles = "Admin, Super Admin")]
        public async Task<ActionResult> GetUpcomingVouchers()
        {
            try
            {
                var vouchers = await _voucherAdminServiceManager.GetUpcomingVouchersAsync();
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Retrieved upcoming vouchers successfully",
                    Data = vouchers
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

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin, Super Admin")]
        public async Task<ActionResult> GetVoucherAdminById(int id)
        {
            try
            {
                VoucherAdminDTO? voucher = await _voucherAdminServiceManager.GetVoucherAdminByIdAsync(id);
                if (voucher == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Status = false,
                        Message = "Voucher not found"
                    });
                }

                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Retrieved voucher information successfully",
                    Data = voucher
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse
                {
                    Status = false,
                    Message = $"An error occurred while retrieving the voucher: {ex.Message}"
                });
            }
        }

        [HttpPost("create")]
        [Authorize(Roles = "Admin, Super Admin")]
        public async Task<IActionResult> CreateVoucherAdmin([FromBody] VoucherAdminDTO voucherDto)
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
                await _voucherAdminServiceManager.CreateVoucherAdminAsync(voucherDto);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Voucher created successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse
                {
                    Status = false,
                    Message = $"An error occurred while creating the voucher: {ex.Message}"
                });
            }
        }

        [HttpPut("update/{id}")]
        [Authorize(Roles = "Admin, Super Admin")]
        public async Task<IActionResult> UpdateVoucherAdmin(int id, [FromBody] VoucherAdminDTO voucherDto)
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
                bool success = await _voucherAdminServiceManager.UpdateVoucherAdminAsync(id, voucherDto);

                if (!success)
                {
                    return NotFound(new ApiResponse
                    {
                        Status = false,
                        Message = "Voucher not found or update failed"
                    });
                }

                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Voucher updated successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse
                {
                    Status = false,
                    Message = $"An error occurred while updating the voucher: {ex.Message}"
                });
            }
        }

        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "Admin, Super Admin")]
        public async Task<IActionResult> DeleteVoucherAdmin(int id)
        {
            try
            {
                bool success = await _voucherAdminServiceManager.DeleteVoucherAdminAsync(id);

                if (!success)
                {
                    return NotFound(new ApiResponse
                    {
                        Status = false,
                        Message = "Voucher not found or deletion failed"
                    });
                }

                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Voucher deleted successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse
                {
                    Status = false,
                    Message = $"An error occurred while deleting the voucher: {ex.Message}"
                });
            }
        }
    }
}

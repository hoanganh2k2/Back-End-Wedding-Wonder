using BusinessObject.DTOs;
using Microsoft.AspNetCore.Mvc;
using Services;
using WeddingWonderAPI.Helper;
using WeddingWonderAPI.Models.Response;

namespace WeddingWonderAPI.Controllers
{
    [Route("api/contract")]
    [ApiController]
    public class ContractController : ControllerBase
    {
        private readonly ContractService _contractService;

        public ContractController(ContractService contractService)
        {
            _contractService = contractService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateContract([FromBody] ContractDTOCreate ContractDTOCreate)
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
                bool result = await _contractService.CreateContractAsync(ContractDTOCreate);
                return result ? Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Contract created successfully"
                }) : BadRequest(new ApiResponse
                {
                    Status = false,
                    Message = "Failed to create contract"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Status = false,
                    Message = $"{ex.Message}"
                });
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllContracts()
        {
            try
            {
                var contracts = await _contractService.GetAllContractsAsync();
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Retrieved all contracts successfully",
                    Data = contracts
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Status = false,
                    Message = $"{ex.Message}"
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetContractById(int id)
        {
            try
            {
                var contract = await _contractService.GetContractByIdAsync(id);
                return contract != null ? Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Retrieved contract successfully",
                    Data = contract
                }) : NotFound(new ApiResponse
                {
                    Status = false,
                    Message = "Contract not found"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Status = false,
                    Message = $"{ex.Message}"
                });
            }
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateContract(int id, [FromBody] ContractDTOCreate contractDto)
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
                await _contractService.UpdateContractAsync(id, contractDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Status = false,
                    Message = $"{ex.Message}"
                });
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteContract(int id)
        {
            try
            {
                await _contractService.DeleteContractAsync(id);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Contract deleted successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Status = false,
                    Message = $"{ex.Message}"
                });
            }
        }

        [HttpPost("{id}/confirm")]
        public async Task<IActionResult> ConfirmContract(int id, [FromBody] ContractConfirmDTO contractDto)
        {
            try
            {
                var result = await _contractService.ConfirmContractAsync(id, contractDto);
                return result ? Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Contract confirmed successfully"
                }) : BadRequest(new ApiResponse
                {
                    Status = false,
                    Message = "Failed to confirm contract"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Status = false,
                    Message = $"{ex.Message}"
                });
            }
        }

        [HttpPost("{id}/send-otp")]
        public async Task<IActionResult> SendOtp(int id, [FromBody] string userEmail)
        {
            try
            {
                string userName = UserHelper.GetUserName(User);
                await _contractService.SendOtpForContractConfirmationAsync(id, userEmail, userName);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "OTP sent successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Status = false,
                    Message = $"{ex.Message}"
                });
            }
        }
    }
}
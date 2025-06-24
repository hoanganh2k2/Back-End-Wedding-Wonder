using BusinessObject.DTOs;
using DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Services;
using WeddingWonderAPI.Models.Response;

namespace WeddingWonderAPI.Controllers
{
    [Route("api/withdraw")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly TransactionService _transactionService;

        public TransactionController(TransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet("deposits")]
        public async Task<IActionResult> GetAllDeposits([FromQuery] string status)
        {
            try
            {
                var deposits = await _transactionService.GetDepositsByStatusAsync(status);
                return Ok(new ApiResponse { Status = true, Data = deposits });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse { Status = false, Message = ex.Message });
            }
        }

        [HttpGet("withdrawals")]
        public async Task<IActionResult> GetAllWithdrawals([FromQuery] string status)
        {
            try
            {
                var withdrawals = await _transactionService.GetWithdrawalsByStatusAsync(status);
                return Ok(new ApiResponse { Status = true, Data = withdrawals });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse { Status = false, Message = ex.Message });
            }
        }

        [HttpGet("deposits/{userId}")]
        public async Task<IActionResult> GetDepositsByUserId(int userId)
        {
            try
            {
                var deposits = await _transactionService.GetDepositsByUserIdAsync(userId);
                return Ok(new ApiResponse { Status = true, Data = deposits });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse { Status = false, Message = ex.Message });
            }
        }

        [HttpGet("{id}")]

        public async Task<ActionResult> GetWithdrawRequestById(int id)
        {
            try
            {
                TransactionDTO? withdrawRequest = await _transactionService.GetWithdrawalRequestByIdAsync(id);
                if (withdrawRequest == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Status = false,
                        Message = "Withdrawal request not found"
                    });
                }

                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Retrieved withdrawal request successfully",
                    Data = withdrawRequest
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

        [HttpPost("create")]

        public async Task<IActionResult> CreateTransaction([FromBody] TransactionDTO withdrawRequestDto)
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
                await _transactionService.CreateTransactionRequestAsync(
                    userId: (int)withdrawRequestDto.UserId,
                    amount: withdrawRequestDto.Amount ?? 0,
                    transactionType: withdrawRequestDto.TransactionType,
                    status: withdrawRequestDto.Status ?? "1",
                    reason: withdrawRequestDto.Reason,
                    requestDate: withdrawRequestDto.RequestDate,
                    processedDate: withdrawRequestDto.ProcessedDate,
                    cardHolderName: withdrawRequestDto.CardHolderName,
                    cardNumber: withdrawRequestDto.CardNumber,
                    bankName: withdrawRequestDto.BankName
                );
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Request created successfully"
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

        [HttpPut("update/{id}")]

        public async Task<IActionResult> UpdateWithdrawRequest(int id, [FromBody] TransactionDTO withdrawRequestDto)
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
                bool success = await _transactionService.UpdateWithdrawalRequestAsync(id, withdrawRequestDto);

                if (!success)
                {
                    return NotFound(new ApiResponse
                    {
                        Status = false,
                        Message = "Withdrawal request not found or update failed"
                    });
                }

                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Withdrawal request updated successfully"
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

        [HttpDelete("delete/{id}")]

        public async Task<IActionResult> DeleteWithdrawRequest(int id)
        {
            try
            {
                bool success = await _transactionService.DeleteWithdrawalRequestAsync(id);

                if (!success)
                {
                    return NotFound(new ApiResponse
                    {
                        Status = false,
                        Message = "Withdrawal request not found or deletion failed"
                    });
                }

                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Withdrawal request deleted successfully"
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

        [HttpPost("accept/{id}")]
        public async Task<IActionResult> AcpWithdrawRequest(int id)
        {
            try
            {
                bool success = await _transactionService.AcpWithdrawRequestAsync(id);
                if (!success)
                {
                    return NotFound(new ApiResponse { Status = false, Message = "Withdrawal request not found" });
                }

                return Ok(new ApiResponse { Status = true, Message = "Withdrawal request accepted" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse { Status = false, Message = $"Error: {ex.Message}" });
            }
        }

        [HttpPost("reject/{id}")]
        public async Task<IActionResult> RejectWithdrawRequest(int id)
        {
            try
            {
                bool success = await _transactionService.RejectWithdrawRequestAsync(id);
                if (!success)
                {
                    return NotFound(new ApiResponse { Status = false, Message = "Withdrawal request not found" });
                }

                return Ok(new ApiResponse { Status = true, Message = "Withdrawal request rejected" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse { Status = false, Message = $"Error: {ex.Message}" });
            }
        }
    }
}
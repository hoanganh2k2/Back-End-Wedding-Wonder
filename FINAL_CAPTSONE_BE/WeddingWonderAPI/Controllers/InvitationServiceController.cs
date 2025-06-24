using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using WeddingWonderAPI.Helper;
using WeddingWonderAPI.Models.DTO;
using WeddingWonderAPI.Models.Response;

namespace WeddingWonderAPI.Controllers
{
    [Route("api/invitationService")]
    [ApiController]
    public class InvitationServiceController : ControllerBase
    {

        private readonly InvitationServiceManager _invitationServiceManager;

        public InvitationServiceController(IConfiguration configuration, InvitationServiceManager invitationService)
        {
            _invitationServiceManager = invitationService;
        }

        [HttpGet("services")]
        public async Task<ActionResult> GetWeddingCardServices()
        {
            try
            {
                List<InvitationPackageDTO> services = await _invitationServiceManager.GetAllInvitationsAsync();
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Get list wedding card services successfully",
                    Data = services
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

        [HttpGet("getPrice/invitationId={invitationId}/number={number}")]
        public async Task<ActionResult> GetPriceByPackageId(int invitationId, int number)
        {
            try
            {
                decimal? price = await _invitationServiceManager.GetPriceByPackageId(invitationId, number);

                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Get Price Packages Success",
                    Data = price,
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
        public async Task<ActionResult> GetWeddingCardServiceById(int id)
        {
            try
            {
                InvitationPackageDTO? service = await _invitationServiceManager.GetInvitationByIdAsync(id);
                if (service == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Status = false,
                        Message = "Wedding card service not found"
                    });
                }

                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Get Information Wedding Card Service Success",
                    Data = service,
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

        [HttpGet("packages/{serviceId}")]
        public async Task<ActionResult> GetCardPackageByServiceId(int serviceId)
        {
            try
            {
                List<InvitationPackageDTO> packages = await _invitationServiceManager.GetCardPackagesByServiceIdAsync(serviceId);

                if (packages == null || !packages.Any())
                {
                    return NotFound(new ApiResponse
                    {
                        Status = false,
                        Message = "No card packages found for the given service ID."
                    });
                }

                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Get Card Packages Success",
                    Data = packages,
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Status = false,
                    Message = $"An error occurred: {ex.Message}"
                });
            }
        }

        [HttpPost("create")]
        [Authorize(Roles = "Supplier")]
        public async Task<IActionResult> CreateWeddingCardService([FromBody] InvitationPackageDTO serviceDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse
                {
                    Status = false,
                    Message = "Invalid data",
                });
            }
            try
            {
                int supplierId = UserHelper.GetUserId(User);
                bool status = await _invitationServiceManager.CreateInvitationAsync(serviceDto);
                if (status)
                {
                    return Ok(new ApiResponse
                    {
                        Status = true,
                        Message = "Wedding card service created successfully"
                    });
                }
                else
                {
                    return BadRequest(new ApiResponse
                    {
                        Status = false,
                        Message = "Failed to create wedding card service"
                    });
                }
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
        [Authorize(Roles = "Supplier")]
        public async Task<IActionResult> UpdateWeddingCardService(int id, [FromBody] InvitationPackageDTO serviceDto)
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
                bool status = await _invitationServiceManager.UpdateInvitationAsync(id, serviceDto);
                if (status)
                {
                    return Ok(new ApiResponse
                    {
                        Status = true,
                        Message = "Wedding card service updated successfully"
                    });
                }
                else
                {
                    return BadRequest(new ApiResponse
                    {
                        Status = false,
                        Message = "Wedding card service not found or update failed"
                    });
                }
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
        [Authorize(Roles = "Supplier")]
        public async Task<IActionResult> DeleteWeddingCardService(int id)
        {
            try
            {
                bool status = await _invitationServiceManager.DeleteInvitationAsync(id);
                if (status)
                {
                    return Ok(new ApiResponse
                    {
                        Status = true,
                        Message = "Wedding card service has been deleted"
                    });
                }
                else
                {
                    return NotFound(new ApiResponse
                    {
                        Status = false,
                        Message = "Wedding card service not found or delete failed"
                    });
                }
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
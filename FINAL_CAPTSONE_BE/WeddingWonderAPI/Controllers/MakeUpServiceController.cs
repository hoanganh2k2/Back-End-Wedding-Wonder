using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using WeddingWonderAPI.Helper;
using WeddingWonderAPI.Models.DTOs;
using WeddingWonderAPI.Models.Response;

namespace WeddingWonderAPI.Controllers
{
    [Route("api/makeupService")]
    [ApiController]
    public class MakeUpServiceController : ControllerBase
    {
        private readonly MakeUpServiceManager _makeUpServiceManager;

        public MakeUpServiceController(MakeUpServiceManager makeUpService)
        {
            _makeUpServiceManager = makeUpService;
        }

        [HttpGet("services")]
        public async Task<ActionResult> GetMakeUpServices()
        {
            try
            {
                List<MakeUpPackageDTO> services = await _makeUpServiceManager.GetAllMakeUpPackagesAsync();
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Get list makeup services successfully",
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

        [HttpGet("getPrice/{packageId}")]
        public async Task<ActionResult> GetPriceByPackageId(int packageId)
        {
            try
            {
                decimal? price = await _makeUpServiceManager.GetPriceByPackageId(packageId);

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
        public async Task<ActionResult> GetMakeUpServiceById(int id)
        {
            try
            {
                MakeUpPackageDTO? service = await _makeUpServiceManager.GetMakeUpPackagesByIdAsync(id);
                if (service == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Status = false,
                        Message = "Makeup service not found"
                    });
                }

                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Get Information Makeup Service Success",
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
        public async Task<ActionResult> GetMakeUpPackageByServiceId(int serviceId)
        {
            try
            {
                List<MakeUpPackageDTO> packages = await _makeUpServiceManager.GetMakeUpPackagesByServiceIdAsync(serviceId);

                if (packages == null || !packages.Any())
                {
                    return NotFound(new ApiResponse
                    {
                        Status = false,
                        Message = "No make-up packages found for the given service ID."
                    });
                }

                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Get MakeUp Packages Success",
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
        public async Task<IActionResult> CreateMakeUpService([FromBody] MakeUpPackageDTO serviceDto)
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
                bool status = await _makeUpServiceManager.CreateMakeUpPackagesAsync(serviceDto);
                if (status)
                {
                    return Ok(new ApiResponse
                    {
                        Status = true,
                        Message = "Makeup service created successfully"
                    });
                }
                else
                {
                    return BadRequest(new ApiResponse
                    {
                        Status = false,
                        Message = "Failed to create makeup service"
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
        public async Task<IActionResult> UpdateMakeUpService(int id, [FromBody] MakeUpPackageDTO serviceDto)
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
                int supplierId = UserHelper.GetUserId(User);
                bool status = await _makeUpServiceManager.UpdateMakeUpPackagesAsync(id, serviceDto);
                if (status)
                {
                    return Ok(new ApiResponse
                    {
                        Status = true,
                        Message = "Makeup service updated successfully"
                    });
                }
                else
                {
                    return BadRequest(new ApiResponse
                    {
                        Status = false,
                        Message = "Makeup service not found or update failed"
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
        public async Task<IActionResult> DeleteMakeUpService(int id)
        {
            try
            {
                bool status = await _makeUpServiceManager.DeleteMakeUpPackagesAsync(id);
                if (status)
                {
                    return Ok(new ApiResponse
                    {
                        Status = true,
                        Message = "Makeup service has been deleted"
                    });
                }
                else
                {
                    return NotFound(new ApiResponse
                    {
                        Status = false,
                        Message = "Makeup service not found or delete failed"
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

        [HttpGet("packages/eventType/{eventType}")]
        public async Task<ActionResult> GetMakeUpPackagesByEventType(int eventType)
        {
            try
            {
                List<MakeUpPackageDTO> packages = await _makeUpServiceManager.GetMakeUpPackagesByEventTypeAsync(eventType);

                if (packages == null || !packages.Any())
                {
                    return NotFound(new ApiResponse
                    {
                        Status = false,
                        Message = "No make-up packages found for the given event type."
                    });
                }

                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Get MakeUp Packages by Event Type Success",
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
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using WeddingWonderAPI.Helper;
using WeddingWonderAPI.Models.DTOs;
using WeddingWonderAPI.Models.Response;

[Route("api/photographService")]
[ApiController]
public class PhotographServiceController : ControllerBase
{
    private readonly PhotographServiceManager _photographServiceManager;

    public PhotographServiceController(PhotographServiceManager photographService)
    {
        _photographServiceManager = photographService;
    }

    [HttpGet("services")]
    public async Task<ActionResult> GetPhotographServices()
    {
        try
        {
            List<PhotographPackageDTO> services = await _photographServiceManager.GetAllPhotographPackagesAsync();
            return Ok(new ApiResponse
            {
                Status = true,
                Message = "Get list photograph services successfully",
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

    [HttpGet("{id}")]
    public async Task<ActionResult> GetPhotographServiceById(int id)
    {
        try
        {
            PhotographPackageDTO? service = await _photographServiceManager.GetPhotographPackagesByIdAsync(id);
            if (service == null)
            {
                return NotFound(new ApiResponse
                {
                    Status = false,
                    Message = "Photograph service not found"
                });
            }

            return Ok(new ApiResponse
            {
                Status = true,
                Message = "Get Information Photograph Service Success",
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

    [HttpGet("getPrice/preId={preId}/weddingId={weddingId}")]
    public async Task<ActionResult> GetPriceByPackageId(int preId, int weddingId)
    {
        try
        {
            decimal? price = await _photographServiceManager.GetPriceByPackageId(preId, weddingId);

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

    [HttpGet("packages/{serviceId}")]
    public async Task<ActionResult> GetPhotoPackageByServiceId(int serviceId)
    {
        try
        {
            List<PhotographPackageDTO> packages = await _photographServiceManager.GetPhotoPackagesByServiceIdAsync(serviceId);

            if (packages == null || !packages.Any())
            {
                return NotFound(new ApiResponse
                {
                    Status = false,
                    Message = "No photo packages found for the given service ID."
                });
            }

            return Ok(new ApiResponse
            {
                Status = true,
                Message = "Get Photo Packages Success",
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
    public async Task<IActionResult> CreatePhotographService([FromBody] PhotographPackageDTO serviceDto)
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
            bool status = await _photographServiceManager.CreatePhotographPackagesAsync(serviceDto);
            if (status)
            {
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Photograph service created successfully"
                });
            }
            else
            {
                return BadRequest(new ApiResponse
                {
                    Status = false,
                    Message = "Failed to create photograph service"
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
    public async Task<IActionResult> UpdatePhotographService(int id, [FromBody] PhotographPackageDTO serviceDto)
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
            bool status = await _photographServiceManager.UpdatePhotographPackagesAsync(id, serviceDto);
            if (status)
            {
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Photograph service updated successfully"
                });
            }
            else
            {
                return BadRequest(new ApiResponse
                {
                    Status = false,
                    Message = "Photograph service not found or update failed"
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
    public async Task<IActionResult> DeletePhotographService(int id)
    {
        try
        {
            bool status = await _photographServiceManager.DeletePhotographPackagesAsync(id);
            if (status)
            {
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Photograph service has been deleted"
                });
            }
            else
            {
                return NotFound(new ApiResponse
                {
                    Status = false,
                    Message = "Photograph service not found or delete failed"
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
    public async Task<ActionResult> GetPhotographPackagesByEventType(int eventType)
    {
        try
        {
            List<PhotographPackageDTO> packages = await _photographServiceManager.GetPhotographPackagesByEventTypeAsync(eventType);
            if (packages == null || !packages.Any())
            {
                return NotFound(new ApiResponse
                {
                    Status = false,
                    Message = "No photograph packages found for the given event type."
                });
            }

            return Ok(new ApiResponse
            {
                Status = true,
                Message = "Get Photograph Packages by Event Type Success",
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
    [HttpGet("packages/eventType/{eventType}/service/{serviceId}")]
    public async Task<ActionResult> GetPhotographPackagesByEventTypeAndServiceId(int eventType, int serviceId)
    {
        try
        {
            List<PhotographPackageDTO> packages = await _photographServiceManager.GetPhotographPackagesByEventTypeAndServiceIdAsync(eventType, serviceId);

            if (packages == null || !packages.Any())
            {
                return NotFound(new ApiResponse
                {
                    Status = false,
                    Message = "No photograph packages found for the given event type and service ID."
                });
            }

            return Ok(new ApiResponse
            {
                Status = true,
                Message = "Get Photograph Packages by Event Type and Service ID Success",
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
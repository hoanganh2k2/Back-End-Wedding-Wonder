using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using WeddingWonderAPI.Helper;
using WeddingWonderAPI.Models.DTOs;
using WeddingWonderAPI.Models.Response;

namespace WeddingWonderAPI.Controllers
{
    [Route("api/eventService")]
    [ApiController]
    public class EventOrganizerServiceController : ControllerBase
    {
        private readonly EventOrganizeServiceManager _eventServiceManager;

        public EventOrganizerServiceController(EventOrganizeServiceManager eventService)
        {
            _eventServiceManager = eventService;
        }

        [HttpGet("services")]
        public async Task<ActionResult> GetEventServices()
        {
            try
            {
                List<EventPackageDTO> services = await _eventServiceManager.GetAllEventPackagesAsync();
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Get list event services successfully",
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
        public async Task<ActionResult> GetEventPackageServiceById(int id)
        {
            try
            {
                EventPackageDTO? service = await _eventServiceManager.GetEventPackageByIdAsync(id);
                if (service == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Status = false,
                        Message = "Event service not found"
                    });
                }

                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Get Information Event Service Success",
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
        public async Task<ActionResult> GetEventPackageByServiceId(int serviceId)
        {
            try
            {
                List<EventPackageDTO> packages = await _eventServiceManager.GetEventPackagesByServiceIdAsync(serviceId);

                if (packages == null || !packages.Any())
                {
                    return NotFound(new ApiResponse
                    {
                        Status = false,
                        Message = "No event packages found for the given service ID."
                    });
                }

                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Get Event Packages Success",
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

        [HttpGet("getPrice/{packageId}")]
        public async Task<ActionResult> GetPriceByPackageId(int packageId)
        {
            try
            {
                decimal? price = await _eventServiceManager.GetPriceByPackageId(packageId);

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

        [HttpPost("create")]
        [Authorize(Roles = "Supplier")]
        public async Task<IActionResult> CreateEventPackageService([FromBody] EventPackageDTO serviceDto)
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
                bool status = await _eventServiceManager.CreateEventPackageAsync(serviceDto);
                if (status)
                {
                    return Ok(new ApiResponse
                    {
                        Status = true,
                        Message = "Event service created successfully"
                    });
                }
                else
                {
                    return BadRequest(new ApiResponse
                    {
                        Status = false,
                        Message = "Failed to create event service"
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
        public async Task<IActionResult> UpdateEventPackageService(int id, [FromBody] EventPackageDTO serviceDto)
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
                bool status = await _eventServiceManager.UpdateEventPackageAsync(id, serviceDto);
                if (status)
                {
                    return Ok(new ApiResponse
                    {
                        Status = true,
                        Message = "Event service updated successfully"
                    });
                }
                else
                {
                    return BadRequest(new ApiResponse
                    {
                        Status = false,
                        Message = "Event service not found or update failed"
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
        public async Task<IActionResult> DeleteEventPackageService(int id)
        {
            try
            {
                bool status = await _eventServiceManager.DeleteEventPackageAsync(id);
                if (status)
                {
                    return Ok(new ApiResponse
                    {
                        Status = true,
                        Message = "Event service has been deleted"
                    });
                }
                else
                {
                    return NotFound(new ApiResponse
                    {
                        Status = false,
                        Message = "Event service not found or delete failed"
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
        public async Task<ActionResult> GetEventPackagesByEventType(int eventType)
        {
            try
            {
                List<EventPackageDTO> packages = await _eventServiceManager.GetEventPackagesByEventTypeAsync(eventType);
                if (packages == null || !packages.Any())
                {
                    return NotFound(new ApiResponse
                    {
                        Status = false,
                        Message = "No event packages found for the given event type."
                    });
                }

                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Get Event Packages by Event Type Success",
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
        public async Task<ActionResult> GetEventPackagesByEventTypeAndServiceId(int eventType, int serviceId)
        {
            try
            {
                List<EventPackageDTO> packages = await _eventServiceManager.GetEventPackagesByEventTypeAndServiceId(eventType, serviceId);

                if (packages == null || !packages.Any())
                {
                    return NotFound(new ApiResponse
                    {
                        Status = false,
                        Message = "No event packages found"
                    });
                }

                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Get Event Packages Success",
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
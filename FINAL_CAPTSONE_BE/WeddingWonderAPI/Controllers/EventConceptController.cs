using BusinessObject.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using WeddingWonderAPI.Helper;
using WeddingWonderAPI.Models.DTOs;
using WeddingWonderAPI.Models.Response;

namespace WeddingWonderAPI.Controllers
{
    [Route("api/eventConcept")]
    [ApiController]
    public class EventConceptController : ControllerBase
    {
        private readonly EventConceptServiceManager _eventConceptServiceManager;

        public EventConceptController(EventConceptServiceManager eventConceptServiceManager)
        {
            _eventConceptServiceManager = eventConceptServiceManager;
        }

        [HttpGet("all")]
        public async Task<ActionResult> GetAllEventConcepts()
        {
            try
            {
                List<EventConceptDTO> concepts = await _eventConceptServiceManager.GetAllEventConceptsAsync();
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Get all Event Concepts successfully",
                    Data = concepts
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
        public async Task<ActionResult> GetEventConceptById(int id)
        {
            try
            {
                EventConceptDTO? concept = await _eventConceptServiceManager.GetEventConceptByIdAsync(id);
                if (concept == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Status = false,
                        Message = "Event Concept not found"
                    });
                }

                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Get Event Concept by Id success",
                    Data = concept
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

        [HttpGet("package/{packageId}")]
        public async Task<ActionResult> GetEventConceptsByPackageId(int packageId)
        {
            try
            {
                List<EventConceptDTO> concepts = await _eventConceptServiceManager.GetEventConceptsByPackageIdAsync(packageId);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Get Event Concepts by PackageId success",
                    Data = concepts
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
        public async Task<ActionResult> CreateEventConcept([FromBody] EventConceptDTO conceptDto)
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
                bool result = await _eventConceptServiceManager.CreateEventConceptAsync(conceptDto);
                if (result)
                {
                    return Ok(new ApiResponse
                    {
                        Status = true,
                        Message = "Event Concept created successfully"
                    });
                }
                return BadRequest(new ApiResponse
                {
                    Status = false,
                    Message = "Failed to create Event Concept"
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
        [Authorize(Roles = "Supplier")]
        public async Task<ActionResult> UpdateEventConcept(int id, [FromBody] EventConceptDTO conceptDto)
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
                bool result = await _eventConceptServiceManager.UpdateEventConceptAsync(id, conceptDto);
                if (result)
                {
                    return Ok(new ApiResponse
                    {
                        Status = true,
                        Message = "Event Concept updated successfully"
                    });
                }
                return BadRequest(new ApiResponse
                {
                    Status = false,
                    Message = "Event Concept not found or update failed"
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

        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "Supplier")]
        public async Task<ActionResult> DeleteEventConcept(int id)
        {
            try
            {
                bool result = await _eventConceptServiceManager.DeleteEventConceptAsync(id);
                if (result)
                {
                    return Ok(new ApiResponse
                    {
                        Status = true,
                        Message = "Event Concept deleted successfully"
                    });
                }
                return NotFound(new ApiResponse
                {
                    Status = false,
                    Message = "Event Concept not found or delete failed"
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

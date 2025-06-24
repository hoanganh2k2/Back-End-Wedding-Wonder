using BusinessObject.DTOs;
using Microsoft.AspNetCore.Mvc;
using Services;
using WeddingWonderAPI.Models.Response;

namespace WeddingWonderAPI.Controllers
{
    [Route("api/photographer")]
    [ApiController]
    public class PhotographerController : ControllerBase
    {
        private readonly PhotographerService _photographerServiceManager;

        public PhotographerController(PhotographerService photographerServiceManager)
        {
            _photographerServiceManager = photographerServiceManager;
        }

        [HttpGet("all")]
        public async Task<ActionResult> GetPhotographers()
        {
            try
            {
                List<PhotographerDTO> photographers = await _photographerServiceManager.GetAllPhotographersAsync();
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Retrieved photographer information successfully",
                    Data = photographers
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
        public async Task<ActionResult> GetPhotographerById(int id)
        {
            try
            {
                PhotographerDTO? photographer = await _photographerServiceManager.GetPhotographerByIdAsync(id);
                if (photographer == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Status = false,
                        Message = "Photographer not found"
                    });
                }

                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Retrieved photographer information successfully",
                    Data = photographer
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
        public async Task<IActionResult> CreatePhotographer([FromBody] PhotographerDTO photographerDto)
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
                await _photographerServiceManager.CreatePhotographerAsync(photographerDto);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Photographer created successfully"
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
        public async Task<IActionResult> UpdatePhotographer(int id, [FromBody] PhotographerDTO photographerDto)
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
                bool success = await _photographerServiceManager.UpdatePhotographerAsync(id, photographerDto);
                if (!success)
                {
                    return NotFound(new ApiResponse
                    {
                        Status = false,
                        Message = "Photographer not found or update failed"
                    });
                }

                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Photographer updated successfully"
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
        public async Task<IActionResult> DeletePhotographer(int id)
        {
            try
            {
                bool success = await _photographerServiceManager.DeletePhotographerAsync(id);
                if (!success)
                {
                    return NotFound(new ApiResponse
                    {
                        Status = false,
                        Message = "Photographer not found or deletion failed"
                    });
                }

                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Photographer deleted successfully"
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
    }
}

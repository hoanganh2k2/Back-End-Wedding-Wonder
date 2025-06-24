using BusinessObject.DTOs;
using DataAccess.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using WeddingWonderAPI.Helper;
using WeddingWonderAPI.Models.Response;

namespace WeddingWonderAPI.Controllers
{
    [Route("api/makeup-artist")]
    [ApiController]
    public class MakeupArtistController : ControllerBase
    {
        private readonly MakeUpArtistService _makeupArtistService;

        public MakeupArtistController(MakeUpArtistService makeupArtistService)
        {
            _makeupArtistService = makeupArtistService;
        }

        [HttpGet("all")]
        public async Task<ActionResult> GetMakeupArtists()
        {
            try
            {
                var makeupArtists = await _makeupArtistService.GetAllMakeUpArtistsAsync();
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Retrieved makeup artists successfully",
                    Data = makeupArtists
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
        public async Task<ActionResult> GetMakeupArtistById(int id)
        {
            try
            {
                var makeupArtist = await _makeupArtistService.GetMakeUpArtistByIdAsync(id);
                if (makeupArtist == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Status = false,
                        Message = "Makeup artist not found"
                    });
                }

                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Retrieved makeup artist successfully",
                    Data = makeupArtist
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
        public async Task<IActionResult> CreateMakeupArtist([FromBody] MakeUpArtistDTO makeupArtistDto)
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
                await _makeupArtistService.CreateMakeUpArtistAsync(makeupArtistDto);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Makeup artist created successfully"
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
        public async Task<IActionResult> UpdateMakeupArtist(int id, [FromBody] MakeUpArtistDTO makeupArtistDto)
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
                bool success = await _makeupArtistService.UpdateMakeUpArtistAsync(id, makeupArtistDto);
                if (!success)
                {
                    return NotFound(new ApiResponse
                    {
                        Status = false,
                        Message = "Makeup artist not found or update failed"
                    });
                }

                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Makeup artist updated successfully"
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
        public async Task<IActionResult> DeleteMakeupArtist(int id)
        {
            try
            {
                bool success = await _makeupArtistService.DeleteMakeUpArtistAsync(id);
                if (!success)
                {
                    return NotFound(new ApiResponse
                    {
                        Status = false,
                        Message = "Makeup artist not found or deletion failed"
                    });
                }

                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Makeup artist deleted successfully"
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

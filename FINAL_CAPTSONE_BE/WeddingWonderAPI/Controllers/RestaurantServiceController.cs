using BusinessObject.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using WeddingWonderAPI.Models.Response;

namespace WeddingWonderAPI.Controllers
{
    [Route("api/catering")]
    [ApiController]
    public class RestaurantServiceController : ControllerBase
    {
        private readonly RestaurantServiceManager _cateringServiceManager;

        public RestaurantServiceController(RestaurantServiceManager cateringServiceManager)
        {
            _cateringServiceManager = cateringServiceManager;
        }

        [HttpGet("all")]

        public async Task<ActionResult> GetCaterings()
        {
            try
            {
                List<CateringDTO> caterings = await _cateringServiceManager.GetAllCateringsAsync();
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Retrieved catering information successfully",
                    Data = caterings
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

        [HttpGet("getPrice/menuId={menuId}/number={number}")]
        public async Task<ActionResult> GetPriceByPackageId(int menuId, int number)
        {
            try
            {
                decimal? price = await _cateringServiceManager.GetPriceByPackageId(menuId, number);

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
        public async Task<ActionResult> GetCateringById(int id)
        {
            try
            {
                CateringDTO? catering = await _cateringServiceManager.GetCateringByIdAsync(id);
                if (catering == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Status = false,
                        Message = "Catering not found"
                    });
                }

                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Retrieved catering information successfully",
                    Data = catering
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

        [HttpGet("packages/{serviceId}")]
        public async Task<ActionResult> GetCardPackageByServiceId(int serviceId)
        {
            try
            {
                List<CateringDTO> packages = await _cateringServiceManager.GetCateringPackagesByServiceIdAsync(serviceId);

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
        public async Task<IActionResult> CreateCatering([FromBody] CateringDTO cateringDto)
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
                await _cateringServiceManager.CreateCateringAsync(cateringDto);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Catering created successfully"
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
        [Authorize(Roles = "Supplier")]
        public async Task<IActionResult> UpdateCatering(int id, [FromBody] CateringDTO cateringDto)
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
                bool status = await _cateringServiceManager.UpdateCateringAsync(id, cateringDto);

                if (status)
                {
                    return Ok(new ApiResponse
                    {
                        Status = true,
                        Message = "Catering updated successfully"
                    });
                }
                else
                {
                    return NotFound(new ApiResponse
                    {
                        Status = false,
                        Message = "Catering service not found or update failed"
                    });
                }
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
        [Authorize(Roles = "Supplier")]
        public async Task<IActionResult> DeleteCatering(int id)
        {
            try
            {
                bool success = await _cateringServiceManager.DeleteCateringAsync(id);

                if (success)
                {
                    return Ok(new ApiResponse
                    {
                        Status = true,
                        Message = "Catering deleted successfully"
                    });
                }
                else
                {
                    return NotFound(new ApiResponse
                    {
                        Status = false,
                        Message = "Catering not found or deletion failed"
                    });
                }
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

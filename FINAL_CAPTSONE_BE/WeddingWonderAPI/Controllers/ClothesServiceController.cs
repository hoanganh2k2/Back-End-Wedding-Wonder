using BusinessObject.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.IRepositories;
using Services;
using WeddingWonderAPI.Helper;
using WeddingWonderAPI.Models.Response;

namespace WeddingWonderAPI.Controllers
{
    [Route("api/clothesService")]
    [ApiController]
    public class ClothesServiceController : ControllerBase
    {
        private readonly ClothesServiceManager _clothesService;
        private readonly IServiceRepository _serviceRepository;
        private readonly IConfiguration _configuration;

        public ClothesServiceController(ClothesServiceManager clothesService, IServiceRepository serviceRepository, IConfiguration configuration)
        {
            _configuration = configuration;
            _clothesService = clothesService;
            _serviceRepository = serviceRepository;
        }

        [HttpGet("getAllOutfitsOfStore/{id}")]
        public async Task<ActionResult> GetOutfitsOfStore(int id)
        {
            try
            {
                List<OutfitDTO> outfits = await _clothesService.GetAllOutfitsOfStore(id);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = $"Get list outfits successfully",
                    Data = outfits
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

        [HttpGet("getoutfits/serviceId={id}/type={type}")]
        public async Task<ActionResult> GetOutfitsByTypeAndServiceId(int id, int type)
        {
            try
            {
                List<OutfitDTO> outfits = await _clothesService.GetOutfitsByTypeAndServiceId(id, type);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = $"Get list outfits successfully",
                    Data = outfits
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

        [HttpGet("outfit/{id}")]
        public async Task<ActionResult> GetOutfitById(int id)
        {
            try
            {
                OutfitDTO outfit = await _clothesService.GetOutfitById(id);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = $"Get outfit successfully",
                    Data = outfit
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

        [HttpGet("packages/{serviceId}")]
        public async Task<ActionResult> GetOutfitsByServiceId(int serviceId)
        {
            try
            {
                List<OutfitDTO> packages = await _clothesService.GetOutfitPackagesByServiceIdAsync(serviceId);

                if (packages == null || !packages.Any())
                {
                    return NotFound(new ApiResponse
                    {
                        Status = false,
                        Message = "No outfit packages found for the given service ID."
                    });
                }

                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Get Outfit Packages Success",
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

        [HttpPut("outfit/{id}")]
        [Authorize(Roles = "Supplier")]
        public async Task<ActionResult> PutOutfit(int id, [FromBody] OutfitDTO outfitRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse
                {
                    Status = false,
                    Message = "Invalid data",
                });
            }

            if (id != outfitRequest.OutfitId)
            {
                return BadRequest(new ApiResponse
                {
                    Status = false,
                    Message = "ID in the URL does not match ID in the request body.",
                });
            }

            try
            {
                int supplierId = UserHelper.GetUserId(User);
                if (!await _clothesService.CheckOutfitId(id, supplierId))
                    return BadRequest(new ApiResponse
                    {
                        Status = false,
                        Message = "You do not have permission to edit this outfit.",
                    });

                await _clothesService.UpdateOutfit(id, outfitRequest);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Outfit has been updated"
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

        [HttpDelete("outfit/{id}")]
        [Authorize(Roles = "Supplier")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                int supplierId = UserHelper.GetUserId(User);
                if (!await _clothesService.CheckOutfitId(id, supplierId))
                    return BadRequest(new ApiResponse
                    {
                        Status = false,
                        Message = "You do not have permission to delete this outfit.",
                    });

                await _clothesService.DeleteOutfit(id);

                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Outfit has been delete"
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
        public async Task<IActionResult> Post([FromBody] OutfitDTO outfitRequest)
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
                if (!await _serviceRepository.CheckSupplierAndService(supplierId, outfitRequest.ServiceId ?? 0))
                    return BadRequest(new ApiResponse
                    {
                        Status = false,
                        Message = "ServiceId error, supplier cannot create new outfit for this store.",
                    });

                bool status = await _clothesService.CreateOutfit(outfitRequest);

                if (status)
                {
                    return Ok(new ApiResponse
                    {
                        Status = true,
                        Message = "Outfit created successfully"
                    });
                }
                else
                {
                    return BadRequest(new ApiResponse
                    {
                        Status = false,
                        Message = "Failed to create outfit"
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse
                {
                    Status = false,
                    Message = $"An internal error occurred: {ex.Message}"
                });
            }
        }
    }
}

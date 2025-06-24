using BusinessObject.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;
using WeddingWonderAPI.Helper;
using WeddingWonderAPI.Models.Response;

namespace WeddingWonderAPI.Controllers
{
    [Route("api/favorites")]
    [ApiController]
    public class FavoriteServiceController : ControllerBase
    {
        private readonly FavoriteServiceService _favoriteServiceService;

        public FavoriteServiceController(FavoriteServiceService favoriteServiceService)
        {
            _favoriteServiceService = favoriteServiceService;
        }
         
        [HttpPost("{serviceId}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> AddFavorite(int serviceId)
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
                int userId = UserHelper.GetUserId(User);
                await _favoriteServiceService.AddFavoriteAsync(userId, serviceId);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Added to favorites successfully"
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


        [HttpDelete("{favoriteId}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> RemoveFavorite(int favoriteId)
        {  
            try
            {
                int userId = UserHelper.GetUserId(User);
                await _favoriteServiceService.RemoveFavoriteAsync(favoriteId, userId);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Removed from favorites successfully"
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

        [HttpGet]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetFavorites()
        {
            int userId = UserHelper.GetUserId(User);
            var favorites = await _favoriteServiceService.GetFavoriteServicesAsync(userId);
            return Ok(new ApiResponse
            {
                Status = true,
                Message = "Favorites retrieved successfully",
                Data = favorites
            });
        }
    }
}

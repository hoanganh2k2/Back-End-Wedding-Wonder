using BusinessObject.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using WeddingWonderAPI.Models.Response;

namespace WeddingWonderAPI.Controllers
{
    [Route("api/menu")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly MenuService _menuService;

        public MenuController(MenuService menuService)
        {
            _menuService = menuService;
        }

        [HttpGet("all")]
        public async Task<ActionResult> GetMenus()
        {
            try
            {
                List<MenuDTO> menus = await _menuService.GetAllMenusAsync();
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Retrieved menu information successfully",
                    Data = menus
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse
                {
                    Status = false,
                    Message = $"An error occurred while retrieving menus: {ex.Message}"
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetMenuById(int id)
        {
            try
            {
                MenuDTO? menu = await _menuService.GetMenuByIdAsync(id);
                if (menu == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Status = false,
                        Message = "Menu not found"
                    });
                }

                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Retrieved menu information successfully",
                    Data = menu
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse
                {
                    Status = false,
                    Message = $"An error occurred while retrieving the menu: {ex.Message}"
                });
            }
        }

        [HttpGet("byCateringId/{cateringId}")]
        public async Task<IActionResult> GetMenusByCateringId(int cateringId)
        {
            try
            {
                List<MenuDTO> menus = await _menuService.GetMenusByCateringIdAsync(cateringId);
                if (menus == null || !menus.Any())
                {
                    return NotFound(new ApiResponse
                    {
                        Status = false,
                        Message = "No menus found for the specified cateringId."
                    });
                }

                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Menus retrieved successfully",
                    Data = menus
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse
                {
                    Status = false,
                    Message = $"An error occurred while retrieving the menus: {ex.Message}"
                });
            }
        }

        [HttpPost("create")]
        [Authorize(Roles = "Supplier")]
        public async Task<IActionResult> CreateMenu([FromBody] MenuDTO menuDto)
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
                await _menuService.CreateMenuAsync(menuDto);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Menu created successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse
                {
                    Status = false,
                    Message = $"An error occurred while creating the menu: {ex.Message}"
                });
            }
        }

        [HttpPut("update/{id}")]
        [Authorize(Roles = "Supplier")]
        public async Task<IActionResult> UpdateMenu(int id, [FromBody] MenuDTO menuDto)
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
                bool success = await _menuService.UpdateMenuAsync(id, menuDto);

                if (!success)
                {
                    return NotFound(new ApiResponse
                    {
                        Status = false,
                        Message = "Menu not found or update failed"
                    });
                }

                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Menu updated successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse
                {
                    Status = false,
                    Message = $"An error occurred while updating the menu: {ex.Message}"
                });
            }
        }

        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "Supplier")]
        public async Task<IActionResult> DeleteMenu(int id)
        {
            try
            {
                bool success = await _menuService.DeleteMenuAsync(id);

                if (!success)
                {
                    return NotFound(new ApiResponse
                    {
                        Status = false,
                        Message = "Menu not found or deletion failed"
                    });
                }

                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Menu deleted successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse
                {
                    Status = false,
                    Message = $"An error occurred while deleting the menu: {ex.Message}"
                });
            }
        }
    }
}

using BusinessObject.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using WeddingWonderAPI.Helper;
using WeddingWonderAPI.Models.Response;

namespace WeddingWonderAPI.Controllers
{
    [Route("api/busy-schedule")]
    [ApiController]
    public class BusyScheduleController : ControllerBase
    {
        private readonly BusyScheduleService _busyScheduleServiceManager;

        public BusyScheduleController(BusyScheduleService busyScheduleServiceManager)
        {
            _busyScheduleServiceManager = busyScheduleServiceManager;
        }

        [HttpGet("all")]

        public async Task<ActionResult> GetBusySchedules()
        {
            try
            {
                List<BusyScheduleDTO> schedules = await _busyScheduleServiceManager.GetAllBusySchedulesAsync();
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Retrieved busy schedule information successfully",
                    Data = schedules
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
        public async Task<ActionResult> GetBusyScheduleById(int id)
        {
            try
            {
                BusyScheduleDTO? schedule = await _busyScheduleServiceManager.GetBusyScheduleByIdAsync(id);
                if (schedule == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Status = false,
                        Message = "Busy schedule not found"
                    });
                }

                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Retrieved busy schedule information successfully",
                    Data = schedule
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

        [HttpGet("service/{serviceId}")]
        public async Task<ActionResult> GetBusySchedulesByServiceId(int serviceId)
        {
            try
            {
                List<BusyScheduleDTO>? schedules = await _busyScheduleServiceManager.GetBusySchedulesByServiceIdAsync(serviceId);
                if (schedules == null || !schedules.Any())
                {
                    return NotFound(new ApiResponse
                    {
                        Status = false,
                        Message = "No busy schedules found for the given service ID"
                    });
                }

                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Retrieved busy schedules successfully",
                    Data = schedules
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
        [Authorize(Roles = "Supplier")]
        public async Task<IActionResult> CreateBusySchedule([FromBody] BusyScheduleDTO busyScheduleDto)
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
                await _busyScheduleServiceManager.CreateBusyScheduleAsync(busyScheduleDto);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Busy schedule created successfully"
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
        public async Task<IActionResult> UpdateBusySchedule(int id, [FromBody] BusyScheduleDTO busyScheduleDto)
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
                bool success = await _busyScheduleServiceManager.UpdateBusyScheduleAsync(id, busyScheduleDto);
                if (!success)
                {
                    return NotFound(new ApiResponse
                    {
                        Status = false,
                        Message = "Busy schedule not found or update failed"
                    });
                }

                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Busy schedule updated successfully"
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
        [Authorize(Roles = "Supplier")]
        public async Task<IActionResult> DeleteBusySchedule(int id)
        {
            try
            {
                bool success = await _busyScheduleServiceManager.DeleteBusyScheduleAsync(id);
                if (!success)
                {
                    return NotFound(new ApiResponse
                    {
                        Status = false,
                        Message = "Busy schedule not found or deletion failed"
                    });
                }

                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Busy schedule deleted successfully"
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
        [HttpGet("supplier")]
        [Authorize(Roles = "Supplier")]
        public async Task<IActionResult> GetBusySchedulesBySupplier()
        {
            int supplierId = UserHelper.GetUserId(User);
            try
            {
                List<BusyScheduleDTO> schedules = await _busyScheduleServiceManager.GetBusySchedulesBySupplierIdAsync(supplierId);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Successfully",
                    Data = schedules
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

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.IRepositories;
using Services;
using WeddingWonderAPI.Helper;
using WeddingWonderAPI.Models.DTOs;
using WeddingWonderAPI.Models.Response;

namespace WeddingWonderAPI.Controllers
{
    [Route("api/service")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly ServiceManager _serviceManager;
        private readonly IElasticService _elasticService;

        public ServiceController(ServiceManager serviceManager, IElasticService elasticService)
        {
            _serviceManager = serviceManager;
            _elasticService = elasticService;
        }

        [HttpGet("services")]
        public async Task<ActionResult> GetServices()
        {
            try
            {
                int userId = UserHelper.GetUserIdForService(User);
                string role = UserHelper.GetRoleForService(User);
                if (role == "guest")
                {
                    userId = 0;
                }
                List<Models.ServiceDTO> services = await _serviceManager.GetAllServicesAsync(userId);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Get list of services successfully",
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

        [HttpGet("services-type")]
        public async Task<ActionResult> GetServicesType()
        {
            try
            {
                int userId = UserHelper.GetUserIdForService(User);
                string role = UserHelper.GetRoleForService(User);
                if (role == "guest")
                {
                    userId = 0;
                }
                List<Models.ServiceDTO> services = await _serviceManager.GetAllServicesAsync(userId);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Get list of services successfully",
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

        [HttpGet("type/{typeId:int}", Name = "GetServiceByTypeId")]
        public async Task<ActionResult> GetServiceByTypeId(int typeId)
        {
            try
            {
                int userId = UserHelper.GetUserIdForService(User);
                string role = UserHelper.GetRoleForService(User);
                if (role == "guest")
                {
                    userId = 0;
                }
                List<Models.ServiceDTO> services = await _serviceManager.GetServicesByTypeId(typeId, userId);
                if (services == null || !services.Any())
                {
                    return NotFound(new ApiResponse
                    {
                        Status = false,
                        Message = "Services not found"
                    });
                }

                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Get Service Information Success",
                    Data = services,
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

        [HttpGet("supplier/type/{serviceTypeId:int}", Name = "GetServicesByUserIdAndType")]
        [Authorize(Roles = "Supplier")]
        public async Task<ActionResult> GetServicesByUserIdAndType(int serviceTypeId)
        {
            try
            {
                int supplierId = UserHelper.GetUserId(User);
                List<Models.ServiceDTO> services = await _serviceManager.GetServicesByUserIdAndTypeAsync(supplierId, serviceTypeId);
                if (services == null || !services.Any())
                {
                    return NotFound(new ApiResponse
                    {
                        Status = false,
                        Message = "No services found for this user and service type"
                    });
                }

                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Services retrieved successfully",
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
        public async Task<ActionResult> GetServiceById(int id)
        {
            try
            {
                Models.ServiceDTO service = await _serviceManager.GetServiceByIdAsync(id);
                if (service == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Status = false,
                        Message = "Service not found"
                    });
                }

                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Get Service Information Success",
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

        [HttpPost("create")]
        [Authorize(Roles = "Supplier")]
        public async Task<IActionResult> CreateService([FromBody] ServiceCreateUpdateRequestDTO request)
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

                bool status = await _serviceManager.CreateServiceAsync(request.BaseService, request.SpecificServiceData, supplierId);
                if (status)
                {
                    //await _elasticService.AddOrUpdateAsync(request.BaseService);
                    return Ok(new ApiResponse
                    {
                        Status = true,
                        Message = "Service created successfully"
                    });
                }
                else
                {
                    return BadRequest(new ApiResponse
                    {
                        Status = false,
                        Message = "Failed to create service"
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
        public async Task<IActionResult> UpdateService(int id, [FromBody] ServiceCreateUpdateRequestDTO request)
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
                bool status = await _serviceManager.UpdateServiceAsync(id, request.BaseService, request.SpecificServiceData, supplierId);
                if (status)
                {
                    await _elasticService.AddOrUpdateAsync(request.BaseService);
                    return Ok(new ApiResponse
                    {
                        Status = true,
                        Message = "Service updated successfully"
                    });
                }
                else
                {
                    return BadRequest(new ApiResponse
                    {
                        Status = false,
                        Message = "Service not found or update failed"
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
        [Authorize(Roles = "Admin, Super Admin, Supplier")]
        public async Task<IActionResult> DeleteService(int id)
        {
            try
            {
                bool status = await _serviceManager.DeleteServiceAsync(id);
                if (status)
                {
                    await _elasticService.RemoveAsync(id);
                    return Ok(new ApiResponse
                    {
                        Status = true,
                        Message = "Service has been deleted"
                    });
                }
                else
                {
                    return NotFound(new ApiResponse
                    {
                        Status = false,
                        Message = "Service not found or delete failed"
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

        [HttpGet("search")]
        public async Task<IActionResult> SearchServices([FromQuery] string keyword, [FromQuery] int? serviceTypeId, [FromQuery] string city)
        {
            try
            {
                List<BusinessObject.Models.Service> services = await _serviceManager.SearchServicesAsync(keyword, serviceTypeId, city);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Services found successfully",
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

        [HttpGet("popular/{count}")]
        public async Task<IActionResult> GetPopularServices(int count)
        {
            try
            {
                List<BusinessObject.Models.Service> services = await _serviceManager.GetPopularServicesAsync(count);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Popular services retrieved successfully",
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

        [HttpGet("statistics/all")]
        [Authorize(Roles = "Admin, Super Admin")]
        public async Task<IActionResult> GetServiceStatistics()
        {
            try
            {
                object statistics = await _serviceManager.GetServiceStatisticsAsync();
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Service statistics retrieved successfully",
                    Data = statistics
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

        [HttpGet("statistics/supplier")]
        [Authorize(Roles = "Supplier")]
        public async Task<IActionResult> GetStatisticsBySupplierId(int supplierId)
        {
            try
            {
                object statistics = await _serviceManager.GetStatisticsBySupplierIdAsync(supplierId);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Service statistics retrieved successfully",
                    Data = statistics
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

        [HttpGet("related/{serviceId}/{count}")]
        public async Task<IActionResult> GetRelatedServices(int serviceId, int count)
        {
            try
            {
                List<BusinessObject.Models.Service> services = await _serviceManager.GetRelatedServicesAsync(serviceId, count);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Related services retrieved successfully",
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

        [HttpGet("supplier/{supplierId}")]
        [Authorize(Roles = "Admin, Super Admin")]
        public async Task<IActionResult> GetServicesBySupplier(int supplierId)
        {
            try
            {
                List<BusinessObject.Models.Service> services = await _serviceManager.GetServicesBySupplierAsync(supplierId);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Supplier services retrieved successfully",
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

        [HttpGet("pending-approval")]
        [Authorize(Roles = "Admin, Super Admin")]
        public async Task<IActionResult> GetPendingApprovalServices()
        {
            try
            {
                List<BusinessObject.Models.Service> services = await _serviceManager.GetPendingApprovalServicesAsync();
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Pending approval services retrieved successfully",
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

        [HttpPut("accept/{serviceId}")]
        [Authorize(Roles = "Admin, Super Admin")]
        public async Task<IActionResult> AcceptService(int serviceId)
        {
            try
            {
                var service = await _serviceManager.AcceptServiceAsync(serviceId);
                if (service != null)
                {
                    await _elasticService.AddOrUpdateAsync(service);

                    return Ok(new ApiResponse
                    {
                        Status = true,
                        Message = "Service accepted successfully and synced to Elasticsearch."
                    });
                }
                else
                {
                    return NotFound(new ApiResponse
                    {
                        Status = false,
                        Message = "Service not found or already accepted."
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

        [HttpDelete("reject/{serviceId}")]
        [Authorize(Roles = "Admin, Super Admin")]
        public async Task<IActionResult> RejectService(int userId, int serviceId, [FromQuery] string reason)
        {
            try
            {
                await _serviceManager.RejectServiceAsync(userId, serviceId, reason);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Service rejected and deleted successfully"
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

        [HttpPost("{serviceId}/addTopics")]
        [Authorize(Roles = "Supplier")]
        public async Task<IActionResult> AddServiceTopics(int serviceId, [FromBody] AddServiceTopicsRequest request)
        {
            try
            {
                bool status = await _serviceManager.AddServiceTopicsAsync(serviceId, request.TopicIds);
                if (status)
                {
                    return Ok(new ApiResponse
                    {
                        Status = true,
                        Message = "Topics added to service successfully"
                    });
                }
                return BadRequest(new ApiResponse
                {
                    Status = false,
                    Message = "Failed to add topics to service"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Status = false,
                    Message = ex.Message
                });
            }
        }
        //bỏ
        [HttpGet("filter")]
        public async Task<IActionResult> FilterServices([FromQuery] int[] serviceTypeIds, [FromQuery] string city, [FromQuery] DateTime? freeScheduleDate, [FromQuery] int[] starNumbers)
        {
            try
            {
                int userId = UserHelper.GetUserIdForService(User);
                List<Models.ServiceDTO> services = await _serviceManager.FilterServicesAsync(serviceTypeIds, city, freeScheduleDate, starNumbers, userId);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Filtered services retrieved successfully",
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
        [HttpGet("filter-elastic")]
        public async Task<IActionResult> FilterServicesElastic([FromQuery] int[] serviceTypeIds, [FromQuery] string city, [FromQuery] DateTime? freeScheduleDate, [FromQuery] int[] starNumbers)
        {
            try
            {
                var services = await _elasticService.SearchAsync(serviceTypeIds, city, freeScheduleDate, starNumbers);

                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Filtered services retrieved successfully",
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
        [HttpPost("sync-elasticsearch")]
        [Authorize]
        //[Authorize(Roles = "Admin, Super Admin")]
        public async Task<IActionResult> SyncToElasticsearch()
        {
            try
            {
                bool status = await _serviceManager.SyncToElasticsearchAsync();
                if (status)
                {
                    return Ok(new ApiResponse
                    {
                        Status = true,
                        Message = "Data successfully synced to Elasticsearch."
                    });
                }

                return BadRequest(new ApiResponse
                {
                    Status = false,
                    Message = "Failed to sync data to Elasticsearch."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Status = false,
                    Message = $"Lỗi rồi =>: {ex.Message}"
                });
            }
        }
        [HttpGet("elastic/all")]
        [Authorize]
        public async Task<IActionResult> GetAllServicesFromElastic()
        {
            try
            {
                var services = await _elasticService.GetAllServicesAsync();

                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "All services retrieved successfully from Elasticsearch.",
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
    }
}
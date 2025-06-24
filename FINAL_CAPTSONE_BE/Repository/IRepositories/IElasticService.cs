using BusinessObject.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeddingWonderAPI.Models;

namespace Repository.IRepositories
{
    public interface IElasticService
    {
        Task<bool> AddOrUpdateAsync(ServiceDTO service);
        Task<bool> AddOrUpdateBulkAsync(IEnumerable<ServiceDTO> services);
        Task<bool> AddOrUpdateBusyScheduleAsync(BusyScheduleDTO schedule);
        Task<IEnumerable<ServiceDTO>> SearchAsync(int[] serviceTypeIds, string city, DateTime? freeScheduleDate, int[] starNumbers);
        Task<bool> RemoveAsync(int serviceId);
        Task<bool> RemoveBusyScheduleAsync(int scheduleId, int serviceId);
        Task<IEnumerable<ServiceDTO>> GetAllServicesAsync();
        Task<ServiceDTO> GetServiceByIdFromElasticAsync(int serviceId);
    }
}

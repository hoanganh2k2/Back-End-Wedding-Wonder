using BusinessObject.Models;
using Repositories.IRepository;

namespace Repository.IRepositories
{
    public interface IServiceRepository : IRepository<Service>
    {
        Task<Service> AcceptService(int serviceId);

        Task<Service> RejectService(int serviceId);

        Task<List<Service>> SearchServices(string keyword, int? serviceTypeId, string city);

        Task<List<Service>> GetPopularServices(int count);

        Task<object> GetServiceStatistics();

        Task<object> GetStatisticsBySupplierId(int supplierId);

        Task<List<Service>> GetServicesByType(int typeId);

        Task<List<Service>> GetsAsyncByUserIdAndType(int userId, int serviceTypeId);

        Task<List<Service>> GetRelatedServices(int serviceId, int count);

        Task<List<Service>> GetServicesBySupplier(int supplierId);

        Task<List<Service>> GetPendingApprovalServices();

        Task UpdateStar(int serviceId, decimal starNew);

        Task<bool> CheckSupplierAndService(int supplierId, int serviceId);

        Task<bool> AddServiceTopics(int serviceId, List<int> topicIds);

        Task<List<Service>> FilterServices(int[] serviceTypeIds, string city, DateTime? freeScheduleDate, int[] starNumbers);
    }
}
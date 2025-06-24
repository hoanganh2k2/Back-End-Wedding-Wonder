using BusinessObject.Models;
using DataAccess;
using Repository.IRepositories;

namespace Repository.Repositories
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly ServiceDAO _dao;

        public ServiceRepository(ServiceDAO dao)
        {
            _dao = dao;
        }

        public Task<bool> CreateAsync(Service obj) => _dao.CreateService(obj);

        public Task DeleteAsync(int id) => _dao.DeleteService(id);

        public Task<Service> GetAsyncById(int id) => _dao.GetServiceById(id);

        public Task<List<Service>> GetsAsync() => _dao.GetServices();

        public Task<List<Service>> GetsAsyncByUserIdAndType(int userId, int serviceTypeId) => _dao.GetServicesByUserIdAndType(userId, serviceTypeId);

        public Task<List<Service>> GetServicesByType(int typeid) => _dao.GetServicesByType(typeid);

        public Task UpdateAsync(int id, Service obj) => _dao.UpdateService(id, obj);

        public Task<Service> RejectService(int serviceId) => _dao.RejectService(serviceId);

        public Task<Service> AcceptService(int serviceId) => _dao.AcceptService(serviceId);

        public Task UpdateStar(int serviceId, decimal starNew) => _dao.UpdateStar(serviceId, starNew);

        public Task<List<Service>> SearchServices(string keyword, int? serviceTypeId, string city) => _dao.SearchServices(keyword, serviceTypeId, city);

        public Task<List<Service>> GetPopularServices(int count) => _dao.GetPopularServices(count);

        public Task<object> GetServiceStatistics() => _dao.GetServiceStatistics();

        public Task<object> GetStatisticsBySupplierId(int supplierId) => _dao.GetStatisticsBySupplierId(supplierId);

        public Task<List<Service>> GetRelatedServices(int serviceId, int count) => _dao.GetRelatedServices(serviceId, count);

        public Task<List<Service>> GetServicesBySupplier(int supplierId) => _dao.GetServicesBySupplier(supplierId);

        public Task<List<Service>> GetPendingApprovalServices() => _dao.GetPendingApprovalServices();

        public Task<bool> CheckSupplierAndService(int supplierId, int serviceId) => _dao.CheckSupplierAndService(supplierId, serviceId);

        public Task<bool> AddServiceTopics(int serviceId, List<int> topicIds)
        => _dao.AddServiceTopics(serviceId, topicIds);

        public Task<List<Service>> FilterServices(int[] serviceTypeIds, string city, DateTime? freeScheduleDate, int[] starNumbers) 
    => _dao.FilterServices(serviceTypeIds, city, freeScheduleDate, starNumbers);
    }
}
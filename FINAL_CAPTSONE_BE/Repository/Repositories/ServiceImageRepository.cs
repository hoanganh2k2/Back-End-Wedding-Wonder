using BusinessObject.Models;
using DataAccess;
using Repository.IRepositories;

namespace Repository.Repositories
{
    public class ServiceImageRepository : IServiceImageRepository
    {
        private readonly ServiceImageDAO _dao;

        public ServiceImageRepository(ServiceImageDAO dao)
        {
            _dao = dao;
        }

        public Task<bool> CreateAsync(ServiceImage obj) => _dao.CreateServiceImage(obj);

        public Task DeleteAsync(int id) => _dao.DeleteServiceImage(id);

        public Task<ServiceImage> GetAsyncById(int id) => _dao.GetServiceImageById(id);

        public Task<ServiceImage> GetFirstImageOfService(int serviceId) => _dao.GetFirstImageOfService(serviceId);
        public Task<List<ServiceImage>> GetImagesByServiceId(int serviceId) => _dao.GetImagesByServiceId(serviceId);

        public Task<List<ServiceImage>> GetsAsync() => _dao.GetServiceImages();

        public Task UpdateAsync(int id, ServiceImage obj) => _dao.UpdateServiceImage(id, obj);
    }

}

using BusinessObject.Models;
using DataAccess;
using Repository.IRepositories;

namespace Repository.Repositories
{
    public class PhotographPackageRepository : IPhotographPackageRepository
    {
        private readonly PhotographPackageDAO _dao;

        public PhotographPackageRepository(PhotographPackageDAO dao)
        {
            _dao = dao;
        }

        public Task<bool> CreateAsync(PhotographPackage obj) => _dao.CreatePhotographPackage(obj);

        public Task DeleteAsync(int id) => _dao.DeletePhotographPackage(id);

        public Task<PhotographPackage> GetAsyncById(int id) => _dao.GetPhotographPackageById(id);

        public Task<List<PhotographPackage>> GetAsyncByServiceId(int serviceid) => _dao.GetPhotoPackageByServiceId(serviceid);

        public Task<List<PhotographPackage>> GetsAsync() => _dao.GetPhotographPackages();

        public Task UpdateAsync(int id, PhotographPackage obj) => _dao.UpdatePhotographPackage(id, obj);

        public Task<List<PhotographPackage>> GetAsyncByEventType(int eventType) => _dao.GetPhotographPackagesByEventType(eventType);

        public Task<List<PhotographPackage>> GetAsyncByEventTypeAndServiceId(int eventType, int serviceId) => _dao.GetPhotographPackagesByEventTypeAndServiceId(eventType, serviceId);
    }
}
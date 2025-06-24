using BusinessObject.Models;
using DataAccess;
using Repository.IRepositories;

namespace Repository.Repositories
{
    public class EventPackageRepository : IEventPackageRepository
    {
        private readonly EventPackageDAO _dao;

        public EventPackageRepository(EventPackageDAO dao)
        {
            _dao = dao;
        }

        public Task<bool> CreateAsync(EventPackage obj) => _dao.CreateEventPackage(obj);

        public Task DeleteAsync(int id) => _dao.DeleteEventPackage(id);

        public Task<EventPackage> GetAsyncById(int id) => _dao.GetEventPackageById(id);

        public Task<List<EventPackage>> GetAsyncByServiceId(int serviceid) => _dao.GetEventPackagesByServiceId(serviceid);

        public Task<EventPackage> GetEventByConceptId(int conceptId) => _dao.GetEventByConceptId(conceptId);

        public Task<List<EventPackage>> GetsAsync() => _dao.GetEventPackages();

        public Task UpdateAsync(int id, EventPackage obj) => _dao.UpdateEventPackage(id, obj);

        public Task<List<EventPackage>> GetEventPackagesByEventType(int eventType) => _dao.GetEventPackagesByEventType(eventType);

        public Task<List<EventPackage>> GetEventPackagesByEventTypeAndServiceId(int eventType, int serviceId) => _dao.GetEventPackagesByEventTypeAndServiceId(eventType, serviceId);
    }
}
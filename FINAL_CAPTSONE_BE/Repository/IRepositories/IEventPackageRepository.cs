using BusinessObject.Models;
using Repositories.IRepository;

namespace Repository.IRepositories
{
    public interface IEventPackageRepository : IRepository<EventPackage>
    {
        Task<List<EventPackage>> GetAsyncByServiceId(int serviceid);
        Task<EventPackage> GetEventByConceptId(int conceptId);
        public Task<List<EventPackage>> GetEventPackagesByEventType(int eventType);
        public Task<List<EventPackage>> GetEventPackagesByEventTypeAndServiceId(int eventType, int serviceId);
    }
}

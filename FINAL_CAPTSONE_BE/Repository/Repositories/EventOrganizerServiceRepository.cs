using BusinessObject.Models;
using DataAccess;
using Repository.IRepositories;

namespace Repository.Repositories
{
    public class EventOrganizerServiceRepository : IEventOrganizerServiceRepository
    {
        private readonly EventOrganizerServiceDAO _dao;
        public EventOrganizerServiceRepository(EventOrganizerServiceDAO dao)
        {
            _dao = dao;
        }
        public Task<bool> CreateAsync(EventOrganizerService obj) => _dao.CreateEventOrganizerService(obj);

        public Task DeleteAsync(int id) => throw new NotImplementedException();

        public Task<EventOrganizerService> GetAsyncById(int id) => _dao.GetEventOrganizerServiceById(id);

        public Task<List<EventOrganizerService>> GetsAsync() => _dao.GetEventOrganizerServices();

        public Task UpdateAsync(int id, EventOrganizerService obj) => _dao.UpdateEventOrganizerService(id, obj);
    }
}

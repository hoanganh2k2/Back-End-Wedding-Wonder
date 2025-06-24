using BusinessObject.Models;
using DataAccess;
using Repository.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class EventConceptRepository : IEventConceptRepository
    {
        private readonly EventConceptDAO _dao;

        public EventConceptRepository(EventConceptDAO dao)
        {
            _dao = dao;
        }
        public Task<bool> CreateAsync(EventConcept obj) => _dao.CreateEventConcept(obj);

        public Task DeleteAsync(int id) => _dao.DeleteEventConcept(id);

        public Task<EventConcept> GetAsyncById(int id) => _dao.GetEventConceptById(id);

        public Task<List<EventConcept>> GetEventConceptsByPackageId(int packageId) => _dao.GetEventConceptsByPackageId(packageId);

        public Task<List<EventConcept>> GetsAsync() => _dao.GetEventConcepts();

        public Task UpdateAsync(int id, EventConcept obj) => _dao.UpdateEventConcept(id, obj);
    }
}

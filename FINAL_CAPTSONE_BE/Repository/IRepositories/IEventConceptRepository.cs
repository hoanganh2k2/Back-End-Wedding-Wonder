using BusinessObject.Models;
using Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepositories
{
    public interface IEventConceptRepository : IRepository<EventConcept>
    {
        public Task<List<EventConcept>> GetEventConceptsByPackageId(int packageId);
    }
}

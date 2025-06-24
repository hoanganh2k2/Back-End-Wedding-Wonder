using BusinessObject.Models;
using Repositories.IRepository;

namespace Repository.IRepositories
{
    public interface IPhotographPackageRepository : IRepository<PhotographPackage>
    {
        Task<List<PhotographPackage>> GetAsyncByServiceId(int serviceid);
        Task<List<PhotographPackage>> GetAsyncByEventType(int eventType);
        Task<List<PhotographPackage>> GetAsyncByEventTypeAndServiceId(int eventType, int serviceId);
    }
}

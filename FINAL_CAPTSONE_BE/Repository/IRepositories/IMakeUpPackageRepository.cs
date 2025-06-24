using BusinessObject.Models;
using Repositories.IRepository;

namespace Repository.IRepositories
{
    public interface IMakeUpPackageRepository : IRepository<MakeUpPackage>
    {
        Task<List<MakeUpPackage>> GetAsyncByServiceId(int serviceid);

        Task<List<MakeUpPackage>> GetAsyncByEventType(int eventType);
    }
}
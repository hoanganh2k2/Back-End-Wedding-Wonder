using BusinessObject.Models;
using Repositories.IRepository;

namespace Repository.IRepositories
{
    public interface IInvitationPackageRepository : IRepository<InvitationPackage>
    {
        Task<List<InvitationPackage>> GetInvitationPackagesByServiceId(int id);
    }
}

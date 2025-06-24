using BusinessObject.Models;
using DataAccess;
using Repository.IRepositories;

namespace Repository.Repositories
{
    public class InvitationPackageRepository : IInvitationPackageRepository
    {
        private readonly InvitationPackageDAO _dao;
        public InvitationPackageRepository(InvitationPackageDAO dao)
        {
            _dao = dao;
        }
        public Task<bool> CreateAsync(InvitationPackage obj) => _dao.CreateInvitationPackage(obj);

        public Task DeleteAsync(int id) => _dao.DeleteInvitationPackage(id);

        public Task<InvitationPackage> GetAsyncById(int id) => _dao.GetInvitationPackageById(id);

        public Task<List<InvitationPackage>> GetInvitationPackagesByServiceId(int id) => _dao.GetInvitationPackagesByServiceId(id);

        public Task<List<InvitationPackage>> GetsAsync() => _dao.GetInvitationPackages();

        public Task UpdateAsync(int id, InvitationPackage obj) => _dao.UpdateInvitationPackage(id, obj);
    }
}

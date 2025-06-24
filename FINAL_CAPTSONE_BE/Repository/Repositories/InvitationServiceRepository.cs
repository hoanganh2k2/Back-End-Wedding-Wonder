using BusinessObject.Models;
using DataAccess;
using Repository.IRepositories;

namespace Repository.Repositories
{
    public class InvitationServiceRepository : IInvitationServiceRepository
    {
        private readonly InvitationServiceDAO _dao;
        public InvitationServiceRepository(InvitationServiceDAO dao)
        {
            _dao = dao;
        }
        public Task<bool> CreateAsync(InvitationService obj) => _dao.CreateInvitation(obj);

        public Task DeleteAsync(int id) => throw new NotImplementedException();

        public Task<InvitationService> GetAsyncById(int id) => _dao.GetInvitationById(id);

        public Task<List<InvitationService>> GetsAsync() => _dao.GetInvitations();

        public Task UpdateAsync(int id, InvitationService obj) => _dao.UpdateInvitation(id, obj);
    }
}

using BusinessObject.Models;
using DataAccess;
using Repository.IRepositories;

namespace Repository.Repositories
{
    public class InForAdminRepository : IInForAdminRepository
    {
        private readonly InForAdminDAO _dao;

        public InForAdminRepository(InForAdminDAO dao)
        {
            _dao = dao;
        }

        public Task<bool> CreateAsync(InforAdmin admin) => _dao.CreateInForAdmin(admin);

        public Task<InforAdmin?> GetByEmailAsync(string email) => _dao.GetInForAdminByEmail(email);

        public Task<List<InforAdmin>> GetsAsync() => _dao.GetInForAdmins();

        public Task UpdateByEmailAsync(string email, InforAdmin admin)
            => _dao.UpdateInForAdmin(email, admin);

        public Task DeleteByEmailAsync(string email) => _dao.DeleteInForAdminByEmail(email);

        public Task<InforAdmin> GetAsyncById(int id) => throw new NotImplementedException();
        public Task UpdateAsync(int id, InforAdmin obj) => throw new NotImplementedException();
        public Task DeleteAsync(int id) => throw new NotImplementedException();
    }
}

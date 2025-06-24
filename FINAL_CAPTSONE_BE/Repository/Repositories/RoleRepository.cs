using BusinessObject.Models;
using DataAccess;
using Repository.IRepositories;

namespace Repository.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly RoleDAO _dao;

        public RoleRepository(RoleDAO dao)
        {
            _dao = dao;
        }

        public Task<bool> CreateAsync(Role obj) => _dao.CreateRole(obj);

        public Task DeleteAsync(int id) => _dao.DeleteRole(id);

        public Task<Role> GetAsyncById(int id) => _dao.GetRoleById(id);

        public Task<List<Role>> GetsAsync() => _dao.GetRoles();

        public Task UpdateAsync(int id, Role obj) => _dao.UpdateRole(id, obj);
    }
}

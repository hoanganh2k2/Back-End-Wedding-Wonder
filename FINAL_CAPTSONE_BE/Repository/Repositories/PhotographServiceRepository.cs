using BusinessObject.Models;
using DataAccess;
using Repository.IRepositories;

namespace Repository.Repositories
{
    public class PhotographServiceRepository : IPhotographServiceRepository
    {
        private readonly PhotographServiceDAO _dao;

        public PhotographServiceRepository(PhotographServiceDAO dao)
        {
            _dao = dao;
        }

        public Task<bool> CreateAsync(PhotographService obj) => _dao.CreatePhotographService(obj);

        public Task DeleteAsync(int id) => throw new NotImplementedException();

        public Task<PhotographService> GetAsyncById(int id) => _dao.GetPhotographServiceById(id);

        public Task<List<PhotographService>> GetsAsync() => _dao.GetPhotographServices();

        public Task UpdateAsync(int id, PhotographService obj) => _dao.UpdatePhotographService(id, obj);
    }
}
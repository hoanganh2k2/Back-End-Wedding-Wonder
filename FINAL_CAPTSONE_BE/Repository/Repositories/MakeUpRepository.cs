using BusinessObject.Models;
using DataAccess;
using Repository.IRepositories;

namespace Repository.Repositories
{
    public class MakeUpServiceRepository : IMakeUpServiceRepository
    {
        private readonly MakeUpServiceDAO _dao;
        public MakeUpServiceRepository(MakeUpServiceDAO dao)
        {
            _dao = dao;
        }
        public Task<bool> CreateAsync(MakeUpService obj) => _dao.CreateMakeUpService(obj);

        public Task DeleteAsync(int id) => throw new NotImplementedException();

        public Task<MakeUpService> GetAsyncById(int id) => _dao.GetMakeUpServiceById(id);

        public Task<List<MakeUpService>> GetsAsync() => _dao.GetMakeupServices();

        public Task UpdateAsync(int id, MakeUpService obj) => _dao.UpdateMakeUpService(id, obj);
    }
}

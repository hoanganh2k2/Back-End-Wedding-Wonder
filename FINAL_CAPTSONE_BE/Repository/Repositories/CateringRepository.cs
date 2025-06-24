using BusinessObject.Models;
using DataAccess;
using Repository.IRepositories;

namespace Repository.Repositories
{
    public class CateringRepository : ICateringRepository
    {
        private readonly CateringDAO _dao;
        public CateringRepository(CateringDAO dao)
        {
            _dao = dao;
        }
        public Task<bool> CreateAsync(Catering obj) => _dao.CreateCatering(obj);

        public Task DeleteAsync(int id) => _dao.DeleteCatering(id);

        public Task<Catering> GetAsyncById(int id) => _dao.GetCateringById(id);

        public Task<List<Catering>> GetAsyncByServiceId(int serviceid) => _dao.GetCateringPackageByServiceId(serviceid);

        public Task<Catering> GetCateringByMenuId(int menuId) => _dao.GetCateringByMenuId(menuId);

        public Task<List<Catering>> GetsAsync() => _dao.GetCaterings();

        public Task UpdateAsync(int id, Catering obj) => _dao.UpdateCatering(id, obj);
    }
}

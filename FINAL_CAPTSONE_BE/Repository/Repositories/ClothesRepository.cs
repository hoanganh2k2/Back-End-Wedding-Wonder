using BusinessObject.Models;
using DataAccess;
using Repository.IRepositories;

namespace Repository.Repositories
{
    public class ClothesRepository : IClothesServiceRepository
    {
        private readonly ClothesServiceDAO _dao;
        public ClothesRepository(ClothesServiceDAO dao)
        {
            _dao = dao;
        }
        public Task<bool> CreateAsync(ClothesService obj) => _dao.CreateClothes(obj);

        public Task DeleteAsync(int id) => throw new NotImplementedException();

        public Task<ClothesService> GetAsyncById(int id) => _dao.GetClothesById(id);

        public Task<List<ClothesService>> GetsAsync() => _dao.GetClothes();

        public Task UpdateAsync(int id, ClothesService obj) => _dao.UpdateClothes(id, obj);
    }
}

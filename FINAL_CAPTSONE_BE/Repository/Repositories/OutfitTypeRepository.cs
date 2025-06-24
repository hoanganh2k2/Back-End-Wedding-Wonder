using BusinessObject.Models;
using DataAccess;
using Repository.IRepositories;

namespace Repository.Repositories
{
    public class OutfitTypeRepository : IOutfitTypeRepository
    {
        private readonly OutfitTypeDAO _dao;
        public OutfitTypeRepository(OutfitTypeDAO dao)
        {
            _dao = dao;
        }
        public Task<bool> CreateAsync(OutfitType obj) => _dao.CreateOutfitType(obj);

        public Task DeleteAsync(int id) => _dao.DeleteOutfitType(id);

        public Task<OutfitType> GetAsyncById(int id) => _dao.GetOutfitTypeById(id);

        public Task<List<OutfitType>> GetsAsync() => _dao.GetOutfitTypes();

        public Task UpdateAsync(int id, OutfitType obj) => _dao.UpdateOutfitType(id, obj);
    }
}

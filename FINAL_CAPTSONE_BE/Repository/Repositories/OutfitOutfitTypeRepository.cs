using BusinessObject.Models;
using DataAccess;
using Repository.IRepositories;

namespace Repository.Repositories
{
    public class OutfitOutfitTypeRepository : IOutfitOutfitTypeRepository
    {
        private readonly OutfitOutfitTypeDAO _dao;
        public OutfitOutfitTypeRepository(OutfitOutfitTypeDAO dao)
        {
            _dao = dao;
        }

        public Task<bool> CreateAsync(OutfitOutfitType obj) => _dao.CreateOutfit(obj);

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task DeleteType(int outfitId, int typeId) => _dao.DeleteOutfit(outfitId, typeId);

        public Task<OutfitOutfitType> GetAsyncById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<int>> GetListsTypeOutfit(int outfitId) => _dao.GetListsTypeOutfit(outfitId);

        public Task<List<OutfitOutfitType>> GetsAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(int id, OutfitOutfitType obj)
        {
            throw new NotImplementedException();
        }
    }
}

using BusinessObject.Models;
using DataAccess;
using Repository.IRepositories;

namespace Repository.Repositories
{
    public class OutfitRepository : IOutfitRepository
    {
        private readonly OutfitDAO _dao;
        public OutfitRepository(OutfitDAO dao)
        {
            _dao = dao;
        }

        public Task<bool> CheckOutfit(int outfitId, int supplierId) => _dao.CheckOutfit(outfitId, supplierId);

        public Task<bool> CreateAsync(Outfit obj) => _dao.CreateOutfit(obj);

        public Task DeleteAsync(int id) => _dao.DeleteOutfit(id);

        public Task<Outfit> GetAsyncById(int id) => _dao.GetOutfitById(id);

        public Task<List<Outfit>> GetOutfitsOfStore(int serviceId) => _dao.GetOutfitsOfStore(serviceId);

        public Task<List<Outfit>> GetAsyncByServiceId(int serviceid) => _dao.GetOutfitByServiceId(serviceid);

        public Task<List<Outfit>> GetsAsync() => _dao.GetOutfits();

        public Task UpdateAsync(int id, Outfit obj) => _dao.UpdateOutfit(id, obj);

        public Task<List<Outfit>> GetOutfitsByTypeOfstore(int type, int serviceId) => _dao.GetOutfitsByTypeOfstore(type, serviceId);
    }
}

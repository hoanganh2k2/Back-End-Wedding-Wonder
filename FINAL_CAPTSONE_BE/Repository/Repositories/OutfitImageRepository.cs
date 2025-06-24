using BusinessObject.Models;
using DataAccess;
using Repository.IRepositories;

namespace Repository.Repositories
{
    public class OutfitImageRepository : IOutfitImageRepository
    {
        private readonly OutfitImageDAO _dao;

        public OutfitImageRepository(OutfitImageDAO dao)
        {
            _dao = dao;
        }
        public Task<bool> CreateAsync(OutfitImage obj) => _dao.CreateOutfitImage(obj);

        public Task DeleteAsync(int id) => _dao.DeleteOutfitImage(id);

        public Task<OutfitImage> GetAsyncById(int id) => _dao.GetImageById(id);

        public Task<OutfitImage> GetFirstImageOfOutfit(int outfitId) => _dao.GetFirstImageOfOutfit(outfitId);

        public Task<List<OutfitImage>> GetImagesByOutfitId(int outfitId) => _dao.GetImagesByOutfitId(outfitId);

        public Task<List<OutfitImage>> GetsAsync() => _dao.GetAllOutfitImages();

        public Task UpdateAsync(int id, OutfitImage obj) => _dao.UpdateOutfitImage(id, obj);
    }
}

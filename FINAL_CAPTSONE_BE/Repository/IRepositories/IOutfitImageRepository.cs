using BusinessObject.Models;
using Repositories.IRepository;

namespace Repository.IRepositories
{
    public interface IOutfitImageRepository : IRepository<OutfitImage>
    {
        Task<List<OutfitImage>> GetImagesByOutfitId(int outfitId);
        Task<OutfitImage> GetFirstImageOfOutfit(int outfitId);
    }
}

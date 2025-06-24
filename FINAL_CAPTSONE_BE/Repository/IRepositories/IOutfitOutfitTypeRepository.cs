using BusinessObject.Models;
using Repositories.IRepository;

namespace Repository.IRepositories
{
    public interface IOutfitOutfitTypeRepository : IRepository<OutfitOutfitType>
    {
        Task<List<int>> GetListsTypeOutfit(int outfitId);
        Task DeleteType(int outfitId, int typeId);
    }
}

using BusinessObject.Models;
using Repositories.IRepository;

namespace Repository.IRepositories
{
    public interface IOutfitRepository : IRepository<Outfit>
    {
        Task<List<Outfit>> GetOutfitsOfStore(int serviceId);
        Task<List<Outfit>> GetOutfitsByTypeOfstore(int type, int serviceId);
        Task<bool> CheckOutfit(int outfitId, int supplierId);
        Task<List<Outfit>> GetAsyncByServiceId(int serviceid);
    }
}

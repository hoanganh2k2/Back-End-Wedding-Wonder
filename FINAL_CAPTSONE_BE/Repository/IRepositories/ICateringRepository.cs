using BusinessObject.Models;
using Repositories.IRepository;

namespace Repository.IRepositories
{
    public interface ICateringRepository : IRepository<Catering>
    {
        Task<List<Catering>> GetAsyncByServiceId(int serviceid);
        Task<Catering> GetCateringByMenuId(int menuId);
    }
}

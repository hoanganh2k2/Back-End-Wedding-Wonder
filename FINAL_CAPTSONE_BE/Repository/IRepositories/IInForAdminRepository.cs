using BusinessObject.Models;
using Repositories.IRepository;

namespace Repository.IRepositories
{
    public interface IInForAdminRepository : IRepository<InforAdmin>
    {
        Task<InforAdmin?> GetByEmailAsync(string email);
        Task UpdateByEmailAsync(string email, InforAdmin admin);
        Task DeleteByEmailAsync(string email);
    }
}

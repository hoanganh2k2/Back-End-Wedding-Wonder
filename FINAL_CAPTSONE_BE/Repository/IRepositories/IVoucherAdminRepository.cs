using BusinessObject.Models;
using Repositories.IRepository;

namespace Repository.IRepositories
{
    public interface IVoucherAdminRepository : IRepository<VoucherAdmin>
    {
        Task<List<VoucherAdmin>> GetUpcomingVouchersAsync();
    }
}

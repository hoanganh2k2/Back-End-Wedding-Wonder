using BusinessObject.Models;
using DataAccess;
using Repository.IRepositories;

namespace Repository.Repositories
{
    public class VoucherAdminRepository : IVoucherAdminRepository
    {
        private readonly VoucherAdminDAO _dao;

        public VoucherAdminRepository(VoucherAdminDAO dao)
        {
            _dao = dao;
        }

        public Task<bool> CreateAsync(VoucherAdmin obj) => _dao.CreateVoucherAdmin(obj);

        public Task DeleteAsync(int id) => _dao.DeleteVoucherAdmin(id);

        public Task<VoucherAdmin> GetAsyncById(int id) => _dao.GetVoucherAdminById(id);

        public Task<List<VoucherAdmin>> GetsAsync() => _dao.GetVoucherAdmins();

        public Task<List<VoucherAdmin>> GetUpcomingVouchersAsync() => _dao.GetUpcomingVouchers();

        public Task UpdateAsync(int id, VoucherAdmin obj) => _dao.UpdateVoucherAdmin(id, obj);
    }
}

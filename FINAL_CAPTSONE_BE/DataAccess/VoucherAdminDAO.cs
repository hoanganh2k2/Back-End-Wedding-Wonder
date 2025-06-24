using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class VoucherAdminDAO
    {
        private readonly WeddingWonderDbContext context;

        public VoucherAdminDAO(WeddingWonderDbContext context)
        {
            this.context = context;
        }

        public async Task<List<VoucherAdmin>> GetVoucherAdmins()
        {
            try
            {
                return await context.VoucherAdmins.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<List<VoucherAdmin>> GetUpcomingVouchers(int days = 7)
        {
            try
            {
                DateTime currentDate = DateTime.Now;
                DateTime targetDate = currentDate.AddDays(days);

                return await context.VoucherAdmins
                    .Where(v => v.EndDate >= currentDate && v.EndDate <= targetDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<VoucherAdmin> GetVoucherAdminById(int voucherId)
        {
            try
            {
                return await context.VoucherAdmins.FindAsync(voucherId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<bool> CreateVoucherAdmin(VoucherAdmin voucherAdmin)
        {
            try
            {
                await context.VoucherAdmins.AddAsync(voucherAdmin);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task UpdateVoucherAdmin(int id, VoucherAdmin voucherAdmin)
        {
            try
            {
                context.VoucherAdmins.Update(voucherAdmin);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task DeleteVoucherAdmin(int voucherId)
        {
            try
            {
                VoucherAdmin? voucherAdminToDelete = await context.VoucherAdmins.FindAsync(voucherId);
                if (voucherAdminToDelete != null)
                {
                    context.VoucherAdmins.Remove(voucherAdminToDelete);
                }
                else
                {
                    throw new Exception("VoucherAdmin not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}

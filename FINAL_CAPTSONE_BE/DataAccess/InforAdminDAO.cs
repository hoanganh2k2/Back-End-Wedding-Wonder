using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class InForAdminDAO
    {
        private readonly WeddingWonderDbContext _context;

        public InForAdminDAO(WeddingWonderDbContext context)
        {
            _context = context;
        }

        public Task<List<InforAdmin>> GetInForAdmins()
            => _context.InforAdmins.ToListAsync();
        public Task<InforAdmin?> GetInForAdminByEmail(string email)
            => _context.InforAdmins.FirstOrDefaultAsync(a => a.Email == email);
        public async Task<bool> CreateInForAdmin(InforAdmin admin)
        {
            try
            {
                await _context.InforAdmins.AddAsync(admin);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task UpdateInForAdmin(string email, InforAdmin updatedAdmin)
        {
            var existingAdmin = await _context.InforAdmins
                .FirstOrDefaultAsync(a => a.Email == email);

            if (existingAdmin == null)
                throw new Exception($"InFoAdmin with email {email} not found.");

            existingAdmin.PhoneNumber = updatedAdmin.PhoneNumber;
            existingAdmin.Address = updatedAdmin.Address;
            existingAdmin.Description = updatedAdmin.Description;

            _context.InforAdmins.Update(existingAdmin);

        }
        public async Task DeleteInForAdminByEmail(string email)
        {
            var adminToDelete = await _context.InforAdmins
                .FirstOrDefaultAsync(a => a.Email == email);

            if (adminToDelete == null)
                throw new Exception($"InFoAdmin with email {email} not found.");

            _context.InforAdmins.Remove(adminToDelete);

        }
    }
}

using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class PhotographServiceDAO
    {
        private readonly WeddingWonderDbContext context;

        public PhotographServiceDAO(WeddingWonderDbContext context)
        {
            this.context = context;
        }

        public async Task<List<PhotographService>> GetPhotographServices()
        {
            using WeddingWonderDbContext context = new();
            return await context.PhotographServices
                .Include(s => s.PhotographPackages)
                .ToListAsync();
        }
        public async Task<PhotographService> GetPhotographServiceById(int photographId)
        {
            try
            {
                PhotographService? photographService = await context.PhotographServices
                    .Include(s => s.PhotographPackages)
                    .FirstOrDefaultAsync(s => s.ServiceId == photographId);

                return photographService ?? throw new Exception("PhotographService not found.");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<bool> CreatePhotographService(PhotographService photographService)
        {
            try
            {
                await context.PhotographServices.AddAsync(photographService);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task UpdatePhotographService(int id, PhotographService photographService)
        {
            try
            {
                context.PhotographServices.Update(photographService);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
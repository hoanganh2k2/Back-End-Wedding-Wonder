using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class MakeUpServiceDAO
    {
        private readonly WeddingWonderDbContext context;

        public MakeUpServiceDAO(WeddingWonderDbContext context)
        {
            this.context = context;
        }

        public async Task<List<MakeUpService>> GetMakeupServices()
        {
            try
            {
                List<MakeUpService> makeUpServices = await context.MakeUpServices
                    .Include(s => s.MakeUpPackages)
                    .ToListAsync();

                return makeUpServices;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<MakeUpService> GetMakeUpServiceById(int makeUpServiceId)
        {
            try
            {
                MakeUpService? makeUpService = await context.MakeUpServices
                    .Include(s => s.MakeUpPackages)
                    .FirstOrDefaultAsync(s => s.ServiceId == makeUpServiceId);

                if (makeUpService == null)
                {
                    throw new Exception("MakeUpService not found.");
                }

                return makeUpService;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<bool> CreateMakeUpService(MakeUpService makeUpService)
        {
            try
            {
                await context.MakeUpServices.AddAsync(makeUpService);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task UpdateMakeUpService(int id, MakeUpService makeUpService)
        {
            try
            {
                var existingService = await context.MakeUpServices.FindAsync(id);
                if (existingService == null)
                    throw new Exception("MakeUpService not found.");

                context.Entry(existingService).CurrentValues.SetValues(makeUpService);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
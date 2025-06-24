using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class MakeUpPackageDAO
    {
        private readonly WeddingWonderDbContext context;
        public MakeUpPackageDAO(WeddingWonderDbContext context)
        {
            this.context = context;
        }
        public async Task<List<MakeUpPackage>> GetMakeUpPackages()
        {
            try
            {
                List<MakeUpPackage> makeUpPackages = await context.MakeUpPackages
                        .ToListAsync();

                return makeUpPackages;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
            public async Task<List<MakeUpPackage>> GetMakeUpPackagesByEventTypeForCombo(int eventType){
             try
              {
                 
                List<MakeUpPackage> makeUpPackages = await context.MakeUpPackages
                .Where(p => p.EventType == eventType && p.Status == true)
              .ToListAsync();

               return makeUpPackages;
              }
               catch (Exception ex)
              {
               throw new Exception("Error retrieving make-up packages by event type: " + ex.Message, ex);
              }
            }
        public async Task<MakeUpPackage> GetMakeUpPackageById(int makeUpPackageId)
        {
            try
            {
                MakeUpPackage? makeUpPackage = await context.MakeUpPackages.FindAsync(makeUpPackageId);

                return makeUpPackage;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<List<MakeUpPackage>> GetMakeUpPackageByServiceId(int serviceId)
        {
            try
            {
                List<MakeUpPackage> makeUpPackages = await context.MakeUpPackages
                    .Where(s => s.ServiceId == serviceId && s.Status == true)
                    .ToListAsync();

                if (makeUpPackages == null || !makeUpPackages.Any())
                {
                    throw new Exception("No MakeUpPackages found for the given ServiceId.");
                }

                return makeUpPackages;
            }
            catch (Exception ex)
            {
                throw new Exception("Error from MakeUpPackageDAO(GetMakeUpPackageByServiceId): " + ex.Message, ex);
            }
        }

        public async Task<Boolean> CreateMakeUpPackage(MakeUpPackage makeUpPackage)
        {
            try
            {
                makeUpPackage.Status = true;
                await context.MakeUpPackages.AddAsync(makeUpPackage);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task UpdateMakeUpPackage(int id, MakeUpPackage makeUpPackage)
        {
            try
            {
                context.MakeUpPackages.Update(makeUpPackage);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task DeleteMakeUpPackage(int makeUpPackageId)
        {
            try
            {
                MakeUpPackage? makeUpPackageToDelete = await context.MakeUpPackages.FindAsync(makeUpPackageId);
                if (makeUpPackageToDelete != null)
                {
                    makeUpPackageToDelete.Status = false;
                    context.MakeUpPackages.Update(makeUpPackageToDelete);
                }
                else
                {
                    throw new Exception("MakeUpPackage not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

    }
}

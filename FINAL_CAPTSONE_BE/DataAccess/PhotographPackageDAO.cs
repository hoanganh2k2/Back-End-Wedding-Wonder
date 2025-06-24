using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class PhotographPackageDAO
    {
        private readonly WeddingWonderDbContext context;

        public PhotographPackageDAO(WeddingWonderDbContext context)
        {
            this.context = context;
        }

        public async Task<List<PhotographPackage>> GetPhotographPackages()
        {
            try
            {
                return await context.PhotographPackages
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<PhotographPackage> GetPhotographPackageById(int photographPackageId)
        {
            try
            {
                PhotographPackage? photographPackage = await context.PhotographPackages.FindAsync(photographPackageId);

                return photographPackage;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<List<PhotographPackage>> GetPhotoPackageByServiceId(int serviceId)
        {
            try
            {
                List<PhotographPackage> photographPackages = await context.PhotographPackages
                    .Where(s => s.ServiceId == serviceId && s.Status == true)
                    .ToListAsync();

                if (photographPackages == null || !photographPackages.Any())
                {
                    throw new Exception("No photoPackages found for the given ServiceId.");
                }

                return photographPackages;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<Boolean> CreatePhotographPackage(PhotographPackage photographPackage)
        {
            try
            {
                photographPackage.Status = true;
                await context.PhotographPackages.AddAsync(photographPackage);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task UpdatePhotographPackage(int id, PhotographPackage photographPackage)
        {
            try
            {
                var existingPackage = await context.PhotographPackages.FindAsync(id);
                if (existingPackage == null)
                    throw new Exception("PhotographPackage not found.");

                context.Entry(existingPackage).CurrentValues.SetValues(photographPackage);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task DeletePhotographPackage(int id)
        {
            try
            {
                PhotographPackage? package = await context.PhotographPackages.FindAsync(id);
                if (package != null)
                {
                    package.Status = false;
                    context.PhotographPackages.Update(package);
                }
                else
                {
                    throw new Exception("Package not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<PhotographPackage>> GetPhotographPackagesByEventType(int eventType)
            {
              try
                 {
                 return await context.PhotographPackages
                    .Where(p => p.EventType == eventType && p.Status == true) 
                   .ToListAsync();
                    }
                  catch (Exception ex)
                   {
                       throw new Exception("Error retrieving photograph packages by event type: " + ex.Message, ex);
                     }
}
        public async Task<List<PhotographPackage>> GetPhotographPackagesByEventTypeAndServiceId(int eventType, int serviceId)
        {
            try
            {
                return await context.PhotographPackages
                    .Where(p => p.EventType == eventType && p.ServiceId == serviceId && p.Status == true)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving photograph packages by event type and serviceId: {ex.Message}", ex);
            }
        }
    }
}
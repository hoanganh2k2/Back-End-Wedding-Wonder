using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class OutfitDAO
    {
        private readonly WeddingWonderDbContext context;
        public OutfitDAO(WeddingWonderDbContext context)
        {
            this.context = context;
        }
        public async Task<List<Outfit>> GetOutfits()
        {
            try
            {
                List<Outfit> outfits = await context.Outfits
                    .Where(o => o.Status == 1 || o.Status == 2)
                    .OrderBy(o => o.Status)
                    .ToListAsync();

                return outfits;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<Outfit>> GetOutfitsByTypeOfstore(int type, int serviceId)
        {
            try
            {
                List<Outfit> outfits = await context.Outfits
                    .Include(o => o.OutfitOutfitTypes)
                    .Where(o => o.ServiceId == serviceId)
                    .Where(o => o.Status == 1 || o.Status == 2)
                    .Where(o => o.OutfitOutfitTypes.Any(oot => oot.OutfitTypeId == type))
                    .OrderBy(o => o.Status)
                    .ToListAsync();

                if (outfits == null || !outfits.Any())
                    throw new Exception("The store has no outfit");

                return outfits;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<Outfit>> GetOutfitsOfStore(int serviceId)
        {
            try
            {
                List<Outfit> outfits = await context.Outfits
                    .Where(s => s.ServiceId == serviceId)
                    .Where(o => o.Status == 1 || o.Status == 2)
                    .OrderBy(o => o.Status)
                    .ToListAsync();

                if (outfits == null || !outfits.Any())
                    throw new Exception("The store has no outfit");

                return outfits;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<Outfit> GetOutfitById(int outfitId)
        {
            try
            {
                Outfit? outfit = await context.Outfits
                    .Where(o => o.Status == 1 || o.Status == 2)
                    .FirstOrDefaultAsync(o => o.OutfitId == outfitId);

                if (outfit == null) throw new Exception("Outfit not found");

                return outfit;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<List<Outfit>> GetOutfitByServiceId(int serviceId)
        {
            try
            {
                List<Outfit> outfitPackages = await context.Outfits
                    .Where(s => s.ServiceId == serviceId && s.Status == 1 || s.Status == 2)
                    .ToListAsync();

                return outfitPackages;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<Boolean> CreateOutfit(Outfit outfit)
        {
            try
            {
                outfit.Status = 1;
                await context.Outfits.AddAsync(outfit);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task UpdateOutfit(int id, Outfit outfit)
        {
            try
            {
                Outfit? outfitInDb = await context.Outfits.FindAsync(id);
                if (outfitInDb == null) throw new Exception("Outfit not found.");

                outfitInDb.OutfitName = outfit.OutfitName;
                outfitInDb.OutfitDescription = outfit.OutfitDescription;
                outfitInDb.OutfitPrice = outfit.OutfitPrice;
                outfitInDb.Status = outfit.Status;

                context.Outfits.Update(outfitInDb);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task DeleteOutfit(int outfitId)
        {
            try
            {
                Outfit? outfitToDelete = await context.Outfits.FindAsync(outfitId);
                if (outfitToDelete != null)
                {
                    outfitToDelete.Status = 0;
                    context.Outfits.Update(outfitToDelete);
                }
                else
                {
                    throw new Exception("Outfit not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<bool> CheckOutfit(int outfitId, int supplierId)
        {
            try
            {
                Outfit? outfitInDb = await context.Outfits.FindAsync(outfitId);
                if (outfitInDb == null) throw new Exception("Outfit not found.");

                Service? service = await context.Services.FindAsync(outfitInDb.ServiceId);

                return service.SupplierId == supplierId;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}

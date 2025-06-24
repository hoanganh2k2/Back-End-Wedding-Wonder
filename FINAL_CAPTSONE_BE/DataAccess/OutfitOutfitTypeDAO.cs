using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class OutfitOutfitTypeDAO
    {
        private readonly WeddingWonderDbContext context;
        public OutfitOutfitTypeDAO(WeddingWonderDbContext context)
        {
            this.context = context;
        }
        public async Task<List<int>> GetListsTypeOutfit(int outfitId)
        {
            try
            {
                List<int> types = await context.OutfitOutfitTypes
                    .Where(o => o.OutfitId == outfitId)
                    .Select(o => o.OutfitTypeId)
                    .ToListAsync();

                return types;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<Boolean> CreateOutfit(OutfitOutfitType types)
        {
            try
            {
                types.CreatedAt = DateTime.Now;
                await context.OutfitOutfitTypes.AddAsync(types);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task DeleteOutfit(int outfitId, int typeId)
        {
            try
            {
                OutfitOutfitType? outfitToDelete = await context.OutfitOutfitTypes
                    .FirstOrDefaultAsync(o => o.OutfitTypeId == typeId && o.OutfitId == outfitId);
                if (outfitToDelete != null)
                {
                    context.OutfitOutfitTypes.Remove(outfitToDelete);
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
    }
}

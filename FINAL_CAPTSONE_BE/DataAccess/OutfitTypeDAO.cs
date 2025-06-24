using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class OutfitTypeDAO
    {
        private readonly WeddingWonderDbContext context;
        public OutfitTypeDAO(WeddingWonderDbContext context)
        {
            this.context = context;
        }
        public async Task<List<OutfitType>> GetOutfitTypes()
        {
            try
            {
                List<OutfitType> outfitTypes = await context.OutfitTypes
                    .Where(o => o.Status == 1)
                    .ToListAsync();

                return outfitTypes;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<OutfitType> GetOutfitTypeById(int typeId)
        {
            try
            {
                OutfitType? type = await context.OutfitTypes
                    .Where(o => o.Status == 1)
                    .FirstOrDefaultAsync(o => o.OutfitTypeId == typeId);

                if (type == null) throw new Exception("Type outfit not found");

                return type;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<Boolean> CreateOutfitType(OutfitType type)
        {
            try
            {
                type.Status = 1;
                await context.OutfitTypes.AddAsync(type);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task UpdateOutfitType(int id, OutfitType type)
        {
            try
            {
                OutfitType? typeInDb = await context.OutfitTypes.FindAsync(id);
                if (typeInDb == null) throw new Exception("Type outfit not found.");

                typeInDb.TypeName = type.TypeName;
                typeInDb.Status = type.Status;

                context.OutfitTypes.Update(typeInDb);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task DeleteOutfitType(int outfitId)
        {
            try
            {
                OutfitType? typeToDelete = await context.OutfitTypes.FindAsync(outfitId);
                if (typeToDelete != null)
                {
                    typeToDelete.Status = 0;
                    context.OutfitTypes.Update(typeToDelete);
                }
                else
                {
                    throw new Exception("Type outfit not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}

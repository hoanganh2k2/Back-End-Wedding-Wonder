using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class ClothesServiceDAO
    {
        private readonly WeddingWonderDbContext context;
        public ClothesServiceDAO(WeddingWonderDbContext context)
        {
            this.context = context;
        }
        public async Task<List<ClothesService>> GetClothes()
        {
            try
            {
                List<ClothesService> clothesServices = await context.ClothesServices
                        .ToListAsync();

                return clothesServices;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<ClothesService> GetClothesById(int clothersId)
        {
            try
            {
                ClothesService? clothesService = await context.ClothesServices.FindAsync(clothersId);

                return clothesService;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<Boolean> CreateClothes(ClothesService clothersService)
        {
            try
            {
                await context.ClothesServices.AddAsync(clothersService);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task UpdateClothes(int id, ClothesService clothersService)
        {
            try
            {
                context.ClothesServices.Update(clothersService);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}

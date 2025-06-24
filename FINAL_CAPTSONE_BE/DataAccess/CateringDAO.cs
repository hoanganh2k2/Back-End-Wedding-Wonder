using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class CateringDAO
    {
        private readonly WeddingWonderDbContext context;
        public CateringDAO(WeddingWonderDbContext context)
        {
            this.context = context;
        }
        public async Task<List<Catering>> GetCaterings()
        {
            try
            {
                List<Catering> caterings = await context.Caterings
                        .ToListAsync();

                return caterings;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<Catering> GetCateringById(int cateringId)
        {
            try
            {
                Catering? catering = await context.Caterings.FindAsync(cateringId);

                return catering;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<Catering> GetCateringByMenuId(int menuId)
        {
            try
            {
                Menu? menu = await context.Menus.Include(m => m.Catering)
                            .FirstOrDefaultAsync(m => m.MenuId == menuId);

                if (menu == null)
                {
                    throw new Exception("Menu not found");
                }

                return menu.Catering;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<Catering>> GetCateringPackageByServiceId(int serviceId)
        {
            try
            {
                List<Catering> cateringPackages = await context.Caterings
                    .Where(s => s.ServiceId == serviceId && s.Status == true)
                    .ToListAsync();

                if (cateringPackages == null || !cateringPackages.Any())
                {
                    throw new Exception("No catering found for the given ServiceId.");
                }

                return cateringPackages;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<Boolean> CreateCatering(Catering catering)
        {
            try
            {
                catering.Status = true;
                await context.Caterings.AddAsync(catering);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task UpdateCatering(int id, Catering catering)
        {
            try
            {
                context.Caterings.Update(catering);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task DeleteCatering(int cateringId)
        {
            try
            {
                Catering? cateringToDelete = await context.Caterings.FindAsync(cateringId);
                if (cateringToDelete != null)
                {
                    cateringToDelete.Status = false;
                    context.Caterings.Update(cateringToDelete);
                }
                else
                {
                    throw new Exception("Catering not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

    }
}

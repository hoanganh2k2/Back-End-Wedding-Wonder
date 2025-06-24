using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class RestaurantServiceDAO
    {
        private readonly WeddingWonderDbContext context;
        public RestaurantServiceDAO(WeddingWonderDbContext context)
        {
            this.context = context;
        }
        public async Task<List<RestaurantService>> GetRestaurantServices()
        {
            try
            {
                List<RestaurantService> restaurantService = await context.RestaurantServices
                        .ToListAsync();

                return restaurantService;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<RestaurantService> GetRestaurantServiceById(int restaurantServiceId)
        {
            try
            {
                RestaurantService restaurantService = await context.RestaurantServices.FindAsync(restaurantServiceId);

                return restaurantService;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<Boolean> CreateRestaurantService(RestaurantService restaurantService)
        {
            try
            {
                await context.RestaurantServices.AddAsync(restaurantService);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task UpdateRestaurantService(int id, RestaurantService restaurantService)
        {
            try
            {
                context.RestaurantServices.Update(restaurantService);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}

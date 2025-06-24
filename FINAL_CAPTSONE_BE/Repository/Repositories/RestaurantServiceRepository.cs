using BusinessObject.Models;
using DataAccess;
using Repository.IRepositories;

namespace Repository.Repositories
{
    public class RestaurantServiceRepository : IRestaurantServiceRepository
    {
        private readonly RestaurantServiceDAO _dao;
        public RestaurantServiceRepository(RestaurantServiceDAO dao)
        {
            _dao = dao;
        }
        public Task<bool> CreateAsync(RestaurantService obj) => _dao.CreateRestaurantService(obj);

        public Task DeleteAsync(int id) => throw new NotImplementedException();

        public Task<RestaurantService> GetAsyncById(int id) => _dao.GetRestaurantServiceById(id);

        public Task<List<RestaurantService>> GetsAsync() => _dao.GetRestaurantServices();

        public Task UpdateAsync(int id, RestaurantService obj) => _dao.UpdateRestaurantService(id, obj);
    }
}

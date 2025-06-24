using BusinessObject.Models;
using DataAccess;
using Repository.IRepositories;

namespace Repository.Repositories
{
    public class CustomerReviewRepository : ICustomerReviewRepository
    {
        private readonly CustomerReviewDAO _dao;

        public CustomerReviewRepository(CustomerReviewDAO dao)
        {
            _dao = dao;
        }

        public Task<bool> CreateAsync(CustomerReview obj) => _dao.CreateCustomerReview(obj);

        public Task DeleteAsync(int id) => _dao.DeleteCustomerReview(id);

        public Task<CustomerReview> GetAsyncById(int id) => _dao.GetCustomerReviewById(id);

        public Task<List<CustomerReview>> GetReviewsByServiceId(int id) => _dao.GetCustomerReviewsByServiceId(id);

        public Task<List<CustomerReview>> GetsAsync() => _dao.GetCustomerReviews();

        public Task<decimal> GetNewAverage(int serviceId, int newStar, int weight) => _dao.GetNewAverage(serviceId, newStar, weight);

        public Task Reply(int id, string reply) => _dao.Reply(id, reply);

        public Task<bool> CanEdit(int id) => _dao.CanEdit(id);

        public Task UpdateAsync(int id, CustomerReview obj) => _dao.UpdateCustomerReview(id, obj);

        public Task<List<CustomerReview>> GetReviewsBySupplierId(int id) => _dao.GetReviewsBySupplierId(id);

        public Task<List<CustomerReview>> GetHistoryReviews(int userId) => _dao.GetHistoryReviews(userId);

        public Task ReplyReview(int id, CustomerReview customerReview) => _dao.ReplyReview(id, customerReview);
    }
}

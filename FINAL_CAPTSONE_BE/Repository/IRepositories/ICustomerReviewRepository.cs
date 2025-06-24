using BusinessObject.Models;
using Repositories.IRepository;

namespace Repository.IRepositories
{
    public interface ICustomerReviewRepository : IRepository<CustomerReview>
    {
        Task<List<CustomerReview>> GetReviewsByServiceId(int id);
        Task<List<CustomerReview>> GetReviewsBySupplierId(int id);
        Task<List<CustomerReview>> GetHistoryReviews(int userId);
        Task ReplyReview(int id, CustomerReview customerReview);
        Task<decimal> GetNewAverage(int serviceId, int newStar, int weight);
        Task Reply(int id, string reply);
        Task<bool> CanEdit(int id);
    }
}

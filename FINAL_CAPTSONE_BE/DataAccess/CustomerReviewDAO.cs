using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class CustomerReviewDAO
    {
        private readonly WeddingWonderDbContext context;

        public CustomerReviewDAO(WeddingWonderDbContext context)
        {
            this.context = context;
        }

        public async Task<List<CustomerReview>> GetCustomerReviews()
        {
            try
            {
                return await context.CustomerReviews
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<CustomerReview>> GetCustomerReviewsByServiceId(int id)
        {
            try
            {
                return await context.CustomerReviews
                    .Where(s => s.ServiceId == id)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<CustomerReview>> GetReviewsBySupplierId(int id)
        {
            try
            {
                return await context.CustomerReviews
                    .Include(r => r.Service)
                    .Where(s => s.Service.SupplierId == id)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<CustomerReview>> GetHistoryReviews(int userId)
        {
            try
            {
                return await context.CustomerReviews
                    .Where(s => s.UserId == userId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<CustomerReview> GetCustomerReviewById(int reviewId)
        {
            try
            {
                CustomerReview? review = await context.CustomerReviews.FindAsync(reviewId);

                if (review == null) throw new Exception("Review not found");


                return review;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<bool> CreateCustomerReview(CustomerReview customerReview)
        {
            try
            {
                customerReview.CanEdit = true;
                await context.CustomerReviews.AddAsync(customerReview);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task UpdateCustomerReview(int id, CustomerReview customerReview)
        {
            try
            {
                CustomerReview? reviewInDb = await context.CustomerReviews.FindAsync(id);
                if (reviewInDb == null) throw new Exception("Review not found.");

                reviewInDb.Content = customerReview.Content;
                reviewInDb.StarNumber = customerReview.StarNumber;
                reviewInDb.CanEdit = false;

                context.CustomerReviews.Update(reviewInDb);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task ReplyReview(int id, CustomerReview customerReview)
        {
            try
            {
                CustomerReview? reviewInDb = await context.CustomerReviews.FindAsync(id);
                if (reviewInDb == null) throw new Exception("Review not found.");

                reviewInDb.Reply = customerReview.Reply;

                context.CustomerReviews.Update(reviewInDb);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task DeleteCustomerReview(int reviewId)
        {
            try
            {
                CustomerReview? customerReviewToDelete = await context.CustomerReviews.FindAsync(reviewId);
                if (customerReviewToDelete != null)
                {
                    context.CustomerReviews.Remove(customerReviewToDelete);
                }
                else
                {
                    throw new Exception("CustomerReview not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<decimal> GetNewAverage(int serviceId, int starNumber, int weight)
        {
            try
            {
                List<int> currentReviews = await context.CustomerReviews
                    .Where(s => s.ServiceId == serviceId)
                    .Select(s => s.StarNumber)
                    .ToListAsync();

                int totalStars = currentReviews.Sum();

                int reviewCount = currentReviews.Count;

                if (reviewCount + weight == 0) return 0;

                decimal newAverage = (decimal)(totalStars + starNumber) / (reviewCount + weight);

                return newAverage;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task Reply(int id, string reply)
        {
            try
            {
                CustomerReview? reviewInDb = await context.CustomerReviews.FindAsync(id);
                if (reviewInDb == null) throw new Exception("Review not found.");

                reviewInDb.Reply = reply;

                context.CustomerReviews.Update(reviewInDb);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<bool> CanEdit(int id)
        {
            try
            {
                CustomerReview? reviewInDb = await context.CustomerReviews.FindAsync(id);
                if (reviewInDb == null) throw new Exception("Review not found.");

                return reviewInDb.CanEdit;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}

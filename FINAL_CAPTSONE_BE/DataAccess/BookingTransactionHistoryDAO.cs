using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class BookingTransactionHistoryDAO
    {
        private readonly WeddingWonderDbContext context;
        public BookingTransactionHistoryDAO(WeddingWonderDbContext context)
        {
            this.context = context;
        }
        public async Task<List<BookingTransactionHistory>> GetTransactionHistories()
        {
            try
            {
                List<BookingTransactionHistory> transactionHistories = await context.BookingTransactionHistories
                    .ToListAsync();

                return transactionHistories;
            }
            catch (Exception ex)
            {
                throw new Exception("Error from BookingTransactionHistoryDAO(GetTransactionHistories): " + ex.Message, ex);
            }
        }
        public async Task<List<BookingTransactionHistory>> GetTransactionHistoriesByBookingId(int bookingId)
        {
            try
            {
                List<BookingTransactionHistory> transactionHistories = await context.BookingTransactionHistories
                    .Where(b => b.BookingId == bookingId)
                    .ToListAsync();

                return transactionHistories;
            }
            catch (Exception ex)
            {
                throw new Exception("Error from BookingTransactionHistoryDAO(GetTransactionHistoriesByBookingId): " + ex.Message, ex);
            }
        }
        public async Task<BookingTransactionHistory> GetTransactionHistoryById(int transactionId)
        {
            try
            {
                BookingTransactionHistory? transactionHistory = await context.BookingTransactionHistories.FindAsync(transactionId);

                return transactionHistory;
            }
            catch (Exception ex)
            {
                throw new Exception("Error from BookingTransactionHistoryDAO(GetTransactionHistoryById): " + ex.Message, ex);
            }
        }
        public async Task<Boolean> CreateTransactionHistory(BookingTransactionHistory transactionHistory)
        {
            try
            {
                await context.BookingTransactionHistories.AddAsync(transactionHistory);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error from BookingTransactionHistoryDAO(CreateTransactionHistory): " + ex.Message, ex);
            }
        }
        public async Task UpdateTransactionHistory(int id, BookingTransactionHistory transactionHistory)
        {
            try
            {
                context.BookingTransactionHistories.Update(transactionHistory);
            }
            catch (Exception ex)
            {
                throw new Exception("Error from BookingTransactionHistoryDAO(UpdateTransactionHistory): " + ex.Message, ex);
            }
        }
    }
}

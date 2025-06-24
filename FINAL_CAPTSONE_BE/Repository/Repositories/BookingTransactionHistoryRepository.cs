using BusinessObject.Models;
using DataAccess;
using Repository.IRepositories;

namespace Repository.Repositories
{
    public class BookingTransactionHistoryRepository : IBookingTransactionHistoryRepository
    {
        private readonly BookingTransactionHistoryDAO _dao;
        public BookingTransactionHistoryRepository(BookingTransactionHistoryDAO dao)
        {
            _dao = dao;
        }
        public Task<bool> CreateAsync(BookingTransactionHistory obj) => _dao.CreateTransactionHistory(obj);

        public Task DeleteAsync(int id) => throw new NotImplementedException();

        public Task<BookingTransactionHistory> GetAsyncById(int id) => _dao.GetTransactionHistoryById(id);

        public Task<List<BookingTransactionHistory>> GetsAsync() => _dao.GetTransactionHistories();

        public Task<List<BookingTransactionHistory>> GetTransactionHistoriesByBookingId(int id) => _dao.GetTransactionHistoriesByBookingId(id);

        public Task UpdateAsync(int id, BookingTransactionHistory obj) => _dao.UpdateTransactionHistory(id, obj);
    }
}

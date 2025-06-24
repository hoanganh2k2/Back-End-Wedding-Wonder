using BusinessObject.Models;
using Repositories.IRepository;

namespace Repository.IRepositories
{
    public interface IBookingTransactionHistoryRepository : IRepository<BookingTransactionHistory>
    {
        Task<List<BookingTransactionHistory>> GetTransactionHistoriesByBookingId(int id);
    }
}

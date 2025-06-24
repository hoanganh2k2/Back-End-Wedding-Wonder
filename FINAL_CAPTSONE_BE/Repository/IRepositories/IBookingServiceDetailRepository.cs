using BusinessObject.Models;
using Repositories.IRepository;

namespace Repository.IRepositories
{
    public interface IBookingServiceDetailRepository : IRepository<BookingServiceDetail>
    {
        Task<Boolean> CreateServiceDetailNotPriority(BookingServiceDetail bookingServiceDetail);
        Task<List<BookingServiceDetail>> GetByBookingId(int id);
        Task<List<BookingServiceDetail>> GetBySupplierId(int supplierId);
        Task<List<BookingServiceDetail>> GetAcceptOfCustomer(int customerId);
        Task<List<BookingServiceDetail>> GetFinishOfCustomer(int customerId);
        Task<List<BookingServiceDetail>> GetCancelOfCustomer(int customerId);
        Task<List<BookingServiceDetail>> GetRequestOfCustomer(int customerId);
        Task<List<BookingServiceDetail>> GetAcceptOfSupplier(int supplierId);
        Task<List<BookingServiceDetail>> GetRejectOfSupplier(int supplierId);
        Task<List<BookingServiceDetail>> GetCancelOfSupplier(int supplierId);
        Task<List<BookingServiceDetail>> GetRequestOfSupplier(int supplierId);
        Task<BookingServiceDetail> GetByBookingIdAndServiceTypeOp1(int bookingId, int serviceTypeId);
        Task<BookingServiceDetail> GetByBookingIdAndServiceTypeOp2(int bookingId, int serviceTypeId);
        Task CancelServiceDetail(int id);
        Task AcceptServiceDetail(int id);
        Task RejectServiceDetail(int id);
        Task UsingServiceDetail(int id);
        Task FinishServiceDetail(int id);
        Task ConfirmFinishServiceDetail(int id);
        Task<User> GetUserByDetailBooking(int detailId);
    }
}

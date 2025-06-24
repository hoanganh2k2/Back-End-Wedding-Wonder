using BusinessObject.Models;
using DataAccess;
using Repository.IRepositories;

namespace Repository.Repositories
{
    public class BookingServiceDetailRepository : IBookingServiceDetailRepository
    {
        private readonly BookingServiceDetailDAO _dao;
        public BookingServiceDetailRepository(BookingServiceDetailDAO dao)
        {
            _dao = dao;
        }
        public Task AcceptServiceDetail(int id) => _dao.AcceptServiceDetail(id);

        public Task<bool> CreateAsync(BookingServiceDetail obj) => _dao.CreateServiceDetail(obj);

        public Task DeleteAsync(int id) => throw new NotImplementedException();

        public Task<BookingServiceDetail> GetAsyncById(int id) => _dao.GetServiceDetailById(id);

        public Task<List<BookingServiceDetail>> GetsAsync() => _dao.GetServiceDetails();

        public Task<List<BookingServiceDetail>> GetByBookingId(int id) => _dao.GetServiceDetailsByBookingId(id);

        public Task<BookingServiceDetail> GetByBookingIdAndServiceTypeOp1(int bookingId, int serviceTypeId) => _dao.GetByBookingIdAndServiceTypeOp1(bookingId, serviceTypeId);

        public Task<BookingServiceDetail> GetByBookingIdAndServiceTypeOp2(int bookingId, int serviceTypeId) => _dao.GetByBookingIdAndServiceTypeOp2(bookingId, serviceTypeId);

        public Task RejectServiceDetail(int id) => _dao.RejectServiceDetail(id);

        public Task UpdateAsync(int id, BookingServiceDetail obj) => _dao.UpdateServiceDetail(id, obj);

        public Task CancelServiceDetail(int id) => _dao.CancelServiceDetail(id);

        public Task UsingServiceDetail(int id) => _dao.UsingServiceDetail(id);

        public Task FinishServiceDetail(int id) => _dao.FinishServiceDetail(id);

        public Task<List<BookingServiceDetail>> GetBySupplierId(int supplierId) => _dao.GetBySupplierId(supplierId);

        public Task<List<BookingServiceDetail>> GetAcceptOfSupplier(int supplierId) => _dao.GetAcceptOfSupplier(supplierId);

        public Task<List<BookingServiceDetail>> GetRejectOfSupplier(int supplierId) => _dao.GetRejectOfSupplier(supplierId);

        public Task<List<BookingServiceDetail>> GetCancelOfSupplier(int supplierId) => _dao.GetCancelOfSupplier(supplierId);

        public Task<List<BookingServiceDetail>> GetRequestOfSupplier(int supplierId) => _dao.GetRequestOfSupplier(supplierId);

        public Task<List<BookingServiceDetail>> GetAcceptOfCustomer(int customerId) => _dao.GetAcceptOfCustomer(customerId);

        public Task<List<BookingServiceDetail>> GetFinishOfCustomer(int customerId) => _dao.GetFinishOfCustomer(customerId);

        public Task<List<BookingServiceDetail>> GetCancelOfCustomer(int customerId) => _dao.GetCancelOfCustomer(customerId);

        public Task<List<BookingServiceDetail>> GetRequestOfCustomer(int customerId) => _dao.GetRequestOfCustomer(customerId);

        public Task ConfirmFinishServiceDetail(int id) => _dao.ConfirmFinishServiceDetail(id);

        public Task<bool> CreateServiceDetailNotPriority(BookingServiceDetail bookingServiceDetail) => _dao.CreateServiceDetailNotPriority(bookingServiceDetail);

        public Task<User> GetUserByDetailBooking(int detailId) => _dao.GetUserByDetailBooking(detailId);
    }
}

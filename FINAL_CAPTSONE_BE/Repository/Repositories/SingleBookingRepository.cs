using BusinessObject.Models;
using DataAccess;
using Repository.IRepositories;

namespace Repository.Repositories
{
    public class SingleBookingRepository : ISingleBookingRepository
    {
        private readonly SingleBookingDAO _dao;
        public SingleBookingRepository(SingleBookingDAO dao)
        {
            _dao = dao;
        }

        public Task AcceptSingleBooking(int id) => _dao.AcceptSingleBooking(id);

        public Task CancelSingleBooking(int id) => _dao.CancelSingleBooking(id);

        public Task<bool> CheckCustomerAndBooking(int bookingId, int customerId) => _dao.CheckCustomerAndBooking(bookingId, customerId);

        public Task<bool> CheckSupplierAndBooking(int bookingId, int supplierId) => _dao.CheckSupplierAndBooking(bookingId, supplierId);

        public Task ConfirmSingleBooking(int id) => _dao.ConfirmSingleBooking(id);

        public Task<int> CountStatusBookingByServiceId(int id, int status) => _dao.CountStatusBookingByServiceId(id, status);

        public Task<int> CountStatusBookingBySupplierId(int id, int status) => _dao.CountStatusBookingBySupplierId(id, status);

        public Task<int> CountStatusBookingByUserId(int id, int status) => _dao.CountStatusBookingByUserId(id, status);
        public Task<bool> CreateAsync(SingleBooking obj) => _dao.CreateSingleBooking(obj);

        public Task DeleteAsync(int id) => throw new NotImplementedException();

        public Task FinishSingleBooking(int id) => _dao.FinishSingleBooking(id);

        public Task<List<SingleBooking>> GetAcceptOfCustomer(int customerId) => _dao.GetAcceptOfCustomer(customerId);

        public Task<List<SingleBooking>> GetAcceptOfSupplier(int supplierId) => _dao.GetAcceptOfSupplier(supplierId);

        public Task<SingleBooking> GetAsyncById(int id) => _dao.GetSingleBookingById(id);

        public Task<List<SingleBooking>> GetCancelOfCustomer(int customerId) => _dao.GetCancelOfCustomer(customerId);

        public Task<List<SingleBooking>> GetCancelOfSupplier(int supplierId) => _dao.GetCancelOfSupplier(supplierId);

        public Task<List<SingleBooking>> GetFinishOfCustomer(int customerId) => _dao.GetFinishOfCustomer(customerId);

        public Task<List<SingleBooking>> GetRejectOfSupplier(int supplierId) => _dao.GetRejectOfSupplier(supplierId);

        public Task<List<SingleBooking>> GetRequestOfCustomer(int customerId) => _dao.GetRequestOfCustomer(customerId);

        public Task<List<SingleBooking>> GetRequestOfSupplier(int supplierId) => _dao.GetRequestOfSupplier(supplierId);

        public Task<List<SingleBooking>> GetsAsync() => _dao.GetSingleBookings();

        public Task<List<SingleBooking>> GetSingleBookingsBySupplierId(int serviceId) => _dao.GetSingleBookingsBySupplierId(serviceId);

        public Task<List<SingleBooking>> GetSingleBookingsByUserId(int userId) => _dao.GetSingleBookingsByUserId(userId);

        public Task RejectSingleBooking(int id) => _dao.RejectSingleBooking(id);

        public Task SuccessSingleBooking(int id) => _dao.SuccessSingleBooking(id);

        public Task UpdateAsync(int id, SingleBooking obj) => _dao.UpdateSingleBooking(id, obj);

        public Task UsingSingleBooking(int id) => _dao.UsingSingleBooking(id);
    }
}

using BusinessObject.DTOs;
using BusinessObject.Models;
using DataAccess;
using Repository.IRepositories;

namespace Repository.Repositories
{
    public class ComboBookingRepository : IComboBookingRepository
    {
        private readonly ComboBookingDAO _dao;
        public ComboBookingRepository(ComboBookingDAO dao)
        {
            _dao = dao;
        }
        public Task AcceptComboBooking(int id) => _dao.AcceptComboBooking(id);

        public Task CancelComboBooking(int id) => _dao.CancelComboBooking(id);

        public Task<bool> CheckCustomerAndBooking(int bookingId, int customerId) => _dao.CheckCustomerAndBooking(bookingId, customerId);

        public Task<bool> CheckCustomerAndDetailBooking(int detailId, int customerId) => _dao.CheckCustomerAndDetailBooking(detailId, customerId);

        public Task<bool> CheckSupplierAndBooking(int detailId, int supplierId) => _dao.CheckSupplierAndDetailBooking(detailId, supplierId);

        public Task ConfirmFinishBooking(int id) => _dao.ConfirmFinishBooking(id);

        public Task<bool> CreateAsync(ComboBooking obj) => _dao.CreateComboBooking(obj);

        public Task DeleteAsync(int id) => throw new NotImplementedException();

        public Task FinishComboBooking(int id) => _dao.FinishComboBooking(id);

        public Task<List<ComboBooking>> GetAcceptOfCustomer(int customerId) => _dao.GetAcceptOfCustomer(customerId);

        public Task<ComboBooking> GetAsyncById(int id) => _dao.GetComboBookingById(id);

        public Task<ComboBooking> GetByDetailId(int detailId) => _dao.GetByDetailId(detailId);

        public Task<List<ComboBooking>> GetByUserId(int userId) => _dao.GetByUserId(userId);

        public Task<List<ComboBooking>> GetCancelOfCustomer(int customerId) => _dao.GetCancelOfCustomer(customerId);

        public Task<List<ComboBooking>> GetFinishOfCustomer(int customerId) => _dao.GetFinishOfCustomer(customerId);

        public Task<List<ComboBooking>> GetRequestOfCustomer(int customerId) => _dao.GetRequestOfCustomer(customerId);

        public Task<List<ComboBooking>> GetsAsync() => _dao.GetComboBookings();

        public Task<User> GetUserByComboId(int comboId) => _dao.GetUserByComboId(comboId);

        public Task RejectComboBooking(int id) => _dao.RejectComboBooking(id);

        public Task UpdateAsync(int id, ComboBooking obj) => _dao.UpdateComboBooking(id, obj);

        public Task UsingComboBooking(int id) => _dao.UsingComboBooking(id);
    }
}

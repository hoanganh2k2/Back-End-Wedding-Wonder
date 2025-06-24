using BusinessObject.Models;
using Repositories.IRepository;

namespace Repository.IRepositories
{
    public interface IComboBookingRepository : IRepository<ComboBooking>
    {
        Task<List<ComboBooking>> GetByUserId(int userId);
        Task<ComboBooking> GetByDetailId(int detailId);
        Task<List<ComboBooking>> GetAcceptOfCustomer(int customerId);
        Task<List<ComboBooking>> GetFinishOfCustomer(int customerId);
        Task<List<ComboBooking>> GetCancelOfCustomer(int customerId);
        Task<List<ComboBooking>> GetRequestOfCustomer(int customerId);
        Task<User> GetUserByComboId(int comboId);
        Task CancelComboBooking(int id);
        Task AcceptComboBooking(int id);
        Task RejectComboBooking(int id);
        Task UsingComboBooking(int id);
        Task FinishComboBooking(int id);
        Task ConfirmFinishBooking(int id);
        Task<bool> CheckCustomerAndBooking(int bookingId, int customerId);
        Task<bool> CheckCustomerAndDetailBooking(int detailId, int customerId);
        Task<bool> CheckSupplierAndBooking(int detailId, int supplierId);
    }
}

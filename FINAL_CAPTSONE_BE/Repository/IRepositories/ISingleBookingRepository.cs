using BusinessObject.Models;
using Repositories.IRepository;

namespace Repository.IRepositories
{
    public interface ISingleBookingRepository : IRepository<SingleBooking>
    {
        Task CancelSingleBooking(int id);
        Task SuccessSingleBooking(int id);
        Task AcceptSingleBooking(int id);
        Task RejectSingleBooking(int id);
        Task UsingSingleBooking(int id);
        Task FinishSingleBooking(int id);
        Task ConfirmSingleBooking(int id);
        Task<List<SingleBooking>> GetSingleBookingsBySupplierId(int serviceId);
        Task<List<SingleBooking>> GetSingleBookingsByUserId(int userId);
        Task<List<SingleBooking>> GetAcceptOfCustomer(int customerId);
        Task<List<SingleBooking>> GetFinishOfCustomer(int customerId);
        Task<List<SingleBooking>> GetCancelOfCustomer(int customerId);
        Task<List<SingleBooking>> GetRequestOfCustomer(int customerId);
        Task<List<SingleBooking>> GetAcceptOfSupplier(int supplierId);
        Task<List<SingleBooking>> GetRejectOfSupplier(int supplierId);
        Task<List<SingleBooking>> GetCancelOfSupplier(int supplierId);
        Task<List<SingleBooking>> GetRequestOfSupplier(int supplierId);
        Task<int> CountStatusBookingBySupplierId(int id, int status);
        Task<int> CountStatusBookingByServiceId(int id, int status);
        Task<int> CountStatusBookingByUserId(int id, int status);
        Task<bool> CheckSupplierAndBooking(int bookingId, int supplierId);
        Task<bool> CheckCustomerAndBooking(int bookingId, int customerId);
    }
}

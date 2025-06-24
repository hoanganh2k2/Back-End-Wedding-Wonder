using BusinessObject.Models;
using DataAccess;
using Repository.IRepositories;

namespace Repository.Repositories
{
    public class InForBookingRepository : IInForBookingRepository
    {
        private readonly InForBookingDAO _dao;

        public InForBookingRepository(InForBookingDAO dao)
        {
            _dao = dao;
        }
        public Task<bool> CreateAsync(InforBooking obj) => _dao.CreateInfor(obj);

        public Task DeleteAsync(int id) => _dao.DeleteInfor(id);

        public Task<InforBooking> GetAsyncById(int id) => _dao.GetInforBookingById(id);

        public Task<List<InforBooking>> GetsAsync() => _dao.GetInforBookings();

        public Task UpdateAsync(int id, InforBooking obj) => _dao.UpdateInfor(id, obj);
    }
}

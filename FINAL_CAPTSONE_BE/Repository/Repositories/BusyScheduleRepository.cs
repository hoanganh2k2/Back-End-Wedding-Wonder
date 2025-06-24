using BusinessObject.Models;
using DataAccess;
using Repository.IRepositories;

namespace Repository.Repositories
{
    public class BusyScheduleRepository : IBusyScheduleRepository
    {
        private readonly BusyScheduleDAO _dao;

        public BusyScheduleRepository(BusyScheduleDAO dao)
        {
            _dao = dao;
        }

        public Task<bool> CreateAsync(BusySchedule obj) => _dao.CreateBusySchedule(obj);

        public Task DeleteAsync(int id) => _dao.DeleteBusySchedule(id);

        public Task<BusySchedule> GetAsyncById(int id) => _dao.GetBusyScheduleById(id);

        public Task<List<BusySchedule>> GetByServiceIdAsync(int serviceId) => _dao.GetBusySchedulesByServiceId(serviceId);

        public Task<List<BusySchedule>> GetBySupplierIdAsync(int supplierId) => _dao.GetBusySchedulesBySupplierId(supplierId);

        public Task<List<BusySchedule>> GetsAsync() => _dao.GetBusySchedules();

        public Task UpdateAsync(int id, BusySchedule obj) => _dao.UpdateBusySchedule(id, obj);
    }
}

using BusinessObject.Models;
using Repositories.IRepository;

namespace Repository.IRepositories
{
    public interface IBusyScheduleRepository : IRepository<BusySchedule>
    {
        Task<List<BusySchedule>> GetByServiceIdAsync(int serviceId);

        Task<List<BusySchedule>> GetBySupplierIdAsync(int supplierId);
    }
}

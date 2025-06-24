using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class BusyScheduleDAO
    {
        private readonly WeddingWonderDbContext context;

        public BusyScheduleDAO(WeddingWonderDbContext context)
        {
            this.context = context;
        }

        public async Task<List<BusySchedule>> GetBusySchedules()
        {
            try
            {
                return await context.BusySchedules
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<BusySchedule> GetBusyScheduleById(int scheduleId)
        {
            try
            {
                return await context.BusySchedules
                    .FindAsync(scheduleId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<BusySchedule>> GetBusySchedulesByServiceId(int serviceId)
        {
            try
            {

                return await context.BusySchedules
                    .Include(s => s.Service)
                    .Where(schedule => schedule.ServiceId == serviceId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<bool> CreateBusySchedule(BusySchedule busySchedule)
        {
            try
            {
                await context.BusySchedules.AddAsync(busySchedule);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task UpdateBusySchedule(int id, BusySchedule busySchedule)
        {
            try
            {
                context.BusySchedules.Update(busySchedule);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task DeleteBusySchedule(int scheduleId)
        {
            try
            {
                BusySchedule? busyScheduleToDelete = await context.BusySchedules.FindAsync(scheduleId);
                if (busyScheduleToDelete != null)
                {
                    context.BusySchedules.Remove(busyScheduleToDelete);
                }
                else
                {
                    throw new Exception("BusySchedule not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<BusySchedule>> GetBusySchedulesBySupplierId(int supplierId)
        {
            try
            {
                return await context.BusySchedules
                    .Include(s => s.Service)  
                    .Where(schedule => schedule.Service.SupplierId == supplierId)  
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}

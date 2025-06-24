using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class InForBookingDAO
    {
        private readonly WeddingWonderDbContext context;

        public InForBookingDAO(WeddingWonderDbContext context)
        {
            this.context = context;
        }

        public async Task<List<InforBooking>> GetInforBookings()
        {
            try
            {
                return await context.InforBookings.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<InforBooking> GetInforBookingById(int id)
        {
            try
            {
                InforBooking? infor = await context.InforBookings.FindAsync(id);

                if (infor == null) throw new Exception("Infor not found");

                return infor;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<Boolean> CreateInfor(InforBooking infor)
        {
            try
            {
                await context.InforBookings.AddAsync(infor);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task UpdateInfor(int id, InforBooking infor)
        {
            try
            {
                InforBooking? inforInDb = await context.InforBookings.FindAsync(id);
                if (inforInDb == null) throw new Exception("Infor not found");

                inforInDb.FullName = infor.FullName;
                inforInDb.PhoneNumber = infor.PhoneNumber;
                inforInDb.City = infor.City;
                inforInDb.District = infor.District;
                inforInDb.Ward = infor.Ward;
                inforInDb.Address = infor.Address;

                context.InforBookings.Update(inforInDb);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task DeleteInfor(int id)
        {
            try
            {
                InforBooking? inforInDb = await context.InforBookings.FindAsync(id);
                if (inforInDb == null) throw new Exception("Infor not found");

                context.InforBookings.Remove(inforInDb);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}

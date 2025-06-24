using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class SingleBookingDAO
    {
        private readonly WeddingWonderDbContext context;
        public SingleBookingDAO(WeddingWonderDbContext context)
        {
            this.context = context;
        }

        public async Task<List<SingleBooking>> GetSingleBookings()
        {
            try
            {
                List<SingleBooking> singleBookings = await context.SingleBookings
                    .OrderByDescending(b => b.CreatedAt)
                    .ToListAsync();

                return singleBookings;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<SingleBooking>> GetSingleBookingsBySupplierId(int supplierId)
        {
            try
            {
                List<SingleBooking> singleBookings = await context.SingleBookings
                    .AsNoTracking()
                    .Include(b => b.Infor)
                    .Include(b => b.User)
                    .Where(b => b.Service.SupplierId == supplierId)
                    .OrderByDescending(b => b.CreatedAt)
                    .ToListAsync();

                return singleBookings;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<SingleBooking>> GetSingleBookingsByUserId(int userId)
        {
            try
            {
                List<SingleBooking> singleBookings = await context.SingleBookings
                    .AsNoTracking()
                    .Include(b => b.Infor)
                    .Include(b => b.User)
                    .Where(b => b.UserId == userId)
                    .OrderByDescending(b => b.CreatedAt)
                    .ToListAsync();

                return singleBookings;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<SingleBooking>> GetAcceptOfCustomer(int customerId)
        {
            try
            {
                List<SingleBooking> singleBookings = await context.SingleBookings
                    .AsNoTracking()
                    .Where(b => b.UserId == customerId && b.BookingStatus == 3)
                    .Include(b => b.Infor)
                    .Include(b => b.User)
                    .OrderByDescending(b => b.CreatedAt)
                    .ToListAsync();

                return singleBookings;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<SingleBooking>> GetFinishOfCustomer(int customerId)
        {
            try
            {
                List<SingleBooking> singleBookings = await context.SingleBookings
                    .AsNoTracking()
                    .Where(b => b.UserId == customerId && (b.BookingStatus == 6 || b.BookingStatus == 7))
                    .Include(b => b.Infor)
                    .Include(b => b.User)
                    .OrderByDescending(b => b.CreatedAt)
                    .ToListAsync();

                return singleBookings;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<SingleBooking>> GetCancelOfCustomer(int customerId)
        {
            try
            {
                List<SingleBooking> singleBookings = await context.SingleBookings
                    .AsNoTracking()
                    .Where(b => b.UserId == customerId && b.BookingStatus == 0)
                    .Include(b => b.Infor)
                    .Include(b => b.User)
                    .OrderByDescending(b => b.CreatedAt)
                    .ToListAsync();

                return singleBookings;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<SingleBooking>> GetRequestOfCustomer(int customerId)
        {
            try
            {
                List<SingleBooking> singleBookings = await context.SingleBookings
                    .AsNoTracking()
                    .Where(b => b.UserId == customerId && (b.BookingStatus == 1 || b.BookingStatus == 2))
                    .Include(b => b.Infor)
                    .Include(b => b.User)
                    .OrderByDescending(b => b.CreatedAt)
                    .ToListAsync();

                return singleBookings;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<SingleBooking>> GetAcceptOfSupplier(int supplierId)
        {
            try
            {
                List<SingleBooking> singleBookings = await context.SingleBookings
                    .AsNoTracking()
                    .Include(b => b.Infor)
                    .Include(b => b.User)
                    .Where(b => b.Service.SupplierId == supplierId && b.BookingStatus == 3)
                    .OrderByDescending(b => b.CreatedAt)
                    .ToListAsync();

                return singleBookings;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<SingleBooking>> GetRejectOfSupplier(int supplierId)
        {
            try
            {
                List<SingleBooking> singleBookings = await context.SingleBookings
                    .AsNoTracking()
                    .Include(b => b.Infor)
                    .Include(b => b.User)
                    .Where(b => b.Service.SupplierId == supplierId && b.BookingStatus == 4)
                    .OrderByDescending(b => b.CreatedAt)
                    .ToListAsync();

                return singleBookings;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<SingleBooking>> GetCancelOfSupplier(int supplierId)
        {
            try
            {
                List<SingleBooking> singleBookings = await context.SingleBookings
                    .AsNoTracking()
                    .Include(b => b.Infor)
                    .Include(b => b.User)
                    .Where(b => b.Service.SupplierId == supplierId && b.BookingStatus == 0)
                    .OrderByDescending(b => b.CreatedAt)
                    .ToListAsync();

                return singleBookings;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<SingleBooking>> GetRequestOfSupplier(int supplierId)
        {
            try
            {
                List<SingleBooking> singleBookings = await context.SingleBookings
                    .AsNoTracking()
                    .Include(b => b.Infor)
                    .Include(b => b.User)
                    .Where(b => b.Service.SupplierId == supplierId && (b.BookingStatus == 1 || b.BookingStatus == 2))
                    .OrderByDescending(b => b.CreatedAt)
                    .ToListAsync();

                return singleBookings;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<SingleBooking> GetSingleBookingById(int singleBookingId)
        {
            try
            {
                SingleBooking? singleBooking = await context.SingleBookings
                    .Include(b => b.Infor)
                    .Include(b => b.User)
                    .Include(b => b.Service)
                    .FirstOrDefaultAsync(b => b.BookingId == singleBookingId);

                return singleBooking;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<Boolean> CreateSingleBooking(SingleBooking singleBooking)
        {
            try
            {
                singleBooking.BookingStatus = 1;
                await context.SingleBookings.AddAsync(singleBooking);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);

            }
        }
        public async Task UpdateSingleBooking(int id, SingleBooking singleBooking)
        {
            try
            {
                context.SingleBookings.Update(singleBooking);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<int> CountStatusBookingBySupplierId(int id, int status)
        {
            try
            {
                return await context.SingleBookings
                    .Include(b => b.Service)
                    .Where(b => b.Service.SupplierId == id)
                    .CountAsync(b => b.BookingStatus == status);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<int> CountStatusBookingByServiceId(int id, int status)
        {
            try
            {
                return await context.SingleBookings
                    .Where(b => b.ServiceId == id)
                    .CountAsync(b => b.BookingStatus == status);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<int> CountStatusBookingByUserId(int id, int status)
        {
            try
            {
                return await context.SingleBookings
                    .Where(b => b.UserId == id)
                    .CountAsync(b => b.BookingStatus == status);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task CancelSingleBooking(int singleBookingId)
        {
            try
            {
                SingleBooking? booking = await context.SingleBookings.FindAsync(singleBookingId);
                if (booking != null)
                {
                    if (booking.BookingStatus >= 1 && booking.BookingStatus <= 3)
                    {
                        booking.BookingStatus = 0;
                        context.SingleBookings.Update(booking);

                    }
                }
                else
                {
                    throw new Exception("Single Booking not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task SuccessSingleBooking(int singleBookingId)
        {
            try
            {
                SingleBooking? booking = await context.SingleBookings.FindAsync(singleBookingId);
                if (booking != null)
                {
                    if (booking.BookingStatus == 1)
                    {
                        booking.BookingStatus = 2;
                        context.SingleBookings.Update(booking);
                    }
                    else throw new Exception("Booking status error");
                }
                else
                {
                    throw new Exception("Single Booking not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task AcceptSingleBooking(int singleBookingId)
        {
            try
            {
                SingleBooking? booking = await context.SingleBookings.FindAsync(singleBookingId);
                if (booking != null)
                {
                    if (booking.BookingStatus == 1 || booking.BookingStatus == 2)
                    {
                        booking.BookingStatus = 3;
                        context.SingleBookings.Update(booking);
                    }
                    else throw new Exception("This booking is not allowed to be accepted.");
                }
                else
                {
                    throw new Exception("Single Booking not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task RejectSingleBooking(int singleBookingId)
        {
            try
            {
                SingleBooking? booking = await context.SingleBookings.FindAsync(singleBookingId);
                if (booking != null)
                {
                    if (booking.BookingStatus == 1 || booking.BookingStatus == 2)
                    {
                        booking.BookingStatus = 4;
                        context.SingleBookings.Update(booking);
                    }
                    else throw new Exception("This booking is not allowed to be rejected.");
                }
                else
                {
                    throw new Exception("Single Booking not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task UsingSingleBooking(int singleBookingId)
        {
            try
            {
                SingleBooking? booking = await context.SingleBookings.FindAsync(singleBookingId);
                if (booking != null)
                {
                    if (booking.BookingStatus == 3)
                    {
                        booking.BookingStatus = 5;
                        context.SingleBookings.Update(booking);
                    }
                    else throw new Exception("This booking cannot be used");
                }
                else
                {
                    throw new Exception("Single Booking not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task FinishSingleBooking(int singleBookingId)
        {
            try
            {
                SingleBooking? booking = await context.SingleBookings.FindAsync(singleBookingId);
                if (booking != null)
                {
                    if (booking.BookingStatus == 5)
                    {
                        booking.BookingStatus = 6;
                        context.SingleBookings.Update(booking);
                    }
                    else throw new Exception("This booking cannot be completed.");
                }
                else
                {
                    throw new Exception("Single Booking not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task ConfirmSingleBooking(int singleBookingId)
        {
            try
            {
                SingleBooking? booking = await context.SingleBookings.FindAsync(singleBookingId);
                if (booking != null)
                {
                    if (booking.BookingStatus == 6)
                    {
                        booking.BookingStatus = 7;
                        context.SingleBookings.Update(booking);
                    }
                    else throw new Exception("This booking cannot be comfirmed.");
                }
                else
                {
                    throw new Exception("Single Booking not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<bool> CheckSupplierAndBooking(int bookingId, int supplierId)
        {
            try
            {
                SingleBooking? booking = await context.SingleBookings
                           .Include(b => b.Service)
                           .FirstOrDefaultAsync(b => b.BookingId == bookingId);
                if (booking == null) throw new Exception("Single Booking not found.");

                return booking.Service.SupplierId == supplierId;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<bool> CheckCustomerAndBooking(int bookingId, int customerId)
        {
            try
            {
                SingleBooking? booking = await context.SingleBookings.FindAsync(bookingId);
                if (booking == null) throw new Exception("Single Booking not found.");

                return booking.UserId == customerId;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}

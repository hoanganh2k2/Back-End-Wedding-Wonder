using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class ComboBookingDAO
    {
        private readonly WeddingWonderDbContext context;
        public ComboBookingDAO(WeddingWonderDbContext context)
        {
            this.context = context;
        }
        public async Task<List<ComboBooking>> GetComboBookings()
        {
            try
            {
                List<ComboBooking> comboBookings = await context.ComboBookings
                    .ToListAsync();

                return comboBookings;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<ComboBooking>> GetByUserId(int userId)
        {
            try
            {
                List<ComboBooking> comboBookings = await context.ComboBookings
                    .Where(c => c.UserId == userId)
                    .ToListAsync();

                return comboBookings;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<ComboBooking>> GetAcceptOfCustomer(int customerId)
        {
            try
            {
                List<ComboBooking> comboBookings = await context.ComboBookings
                    .AsNoTracking()
                    .Where(b => b.UserId == customerId && b.BookingStatus == 2)
                    .Include(b => b.User)
                    .ToListAsync();

                return comboBookings;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<ComboBooking>> GetFinishOfCustomer(int customerId)
        {
            try
            {
                List<ComboBooking> comboBookings = await context.ComboBookings
                    .AsNoTracking()
                    .Where(b => b.UserId == customerId && (b.BookingStatus == 5 || b.BookingStatus == 6))
                    .Include(b => b.User)
                    .ToListAsync();

                return comboBookings;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<ComboBooking>> GetCancelOfCustomer(int customerId)
        {
            try
            {
                List<ComboBooking> comboBookings = await context.ComboBookings
                    .AsNoTracking()
                    .Where(b => b.UserId == customerId && b.BookingStatus == 0)
                    .Include(b => b.User)
                    .ToListAsync();

                return comboBookings;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<ComboBooking>> GetRequestOfCustomer(int customerId)
        {
            try
            {
                List<ComboBooking> comboBookings = await context.ComboBookings
                    .AsNoTracking()
                    .Where(b => b.UserId == customerId && b.BookingStatus == 1)
                    .Include(b => b.User)
                    .ToListAsync();

                return comboBookings;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<ComboBooking> GetComboBookingById(int comboBookingId)
        {
            try
            {
                ComboBooking? comboBooking = await context.ComboBookings.FindAsync(comboBookingId);

                return comboBooking;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<ComboBooking> GetByDetailId(int detailId)
        {
            try
            {
                BookingServiceDetail? detail = await context.BookingServiceDetails
                    .Include(d => d.Booking)
                    .FirstOrDefaultAsync(d => d.DetailId == detailId);

                return detail.Booking;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<User> GetUserByComboId(int comboId)
        {
            try
            {
                ComboBooking? combo = await context.ComboBookings.
                    Include(c => c.User)
                    .FirstOrDefaultAsync(d => d.BookingId == comboId);

                return combo.User;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<Boolean> CreateComboBooking(ComboBooking comboBooking)
        {
            try
            {
                comboBooking.BookingStatus = 1;
                await context.ComboBookings.AddAsync(comboBooking);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task UpdateComboBooking(int id, ComboBooking comboBooking)
        {
            try
            {
                context.ComboBookings.Update(comboBooking);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task CancelComboBooking(int comboBookingId)
        {
            try
            {
                ComboBooking? comboBookingToCancel = await context.ComboBookings.FindAsync(comboBookingId);
                if (comboBookingToCancel != null)
                {
                    comboBookingToCancel.BookingStatus = 0;
                    context.ComboBookings.Update(comboBookingToCancel);
                }
                else
                {
                    throw new Exception("Combo Booking not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task AcceptComboBooking(int comboBookingId)
        {
            try
            {
                ComboBooking? comboBookingToAccept = await context.ComboBookings.FindAsync(comboBookingId);
                if (comboBookingToAccept != null)
                {
                    comboBookingToAccept.BookingStatus = 2;
                    context.ComboBookings.Update(comboBookingToAccept);
                }
                else
                {
                    throw new Exception("Combo Booking not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task RejectComboBooking(int comboBookingId)
        {
            try
            {
                ComboBooking? comboBookingToReject = await context.ComboBookings.FindAsync(comboBookingId);
                if (comboBookingToReject != null)
                {
                    comboBookingToReject.BookingStatus = 3;
                    context.ComboBookings.Update(comboBookingToReject);
                }
                else
                {
                    throw new Exception("Combo Booking not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task UsingComboBooking(int comboBookingId)
        {
            try
            {
                ComboBooking? comboBookingToUsing = await context.ComboBookings.FindAsync(comboBookingId);
                if (comboBookingToUsing != null)
                {
                    comboBookingToUsing.BookingStatus = 4;
                    context.ComboBookings.Update(comboBookingToUsing);
                }
                else
                {
                    throw new Exception("Combo Booking not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task FinishComboBooking(int comboBookingId)
        {
            try
            {
                ComboBooking? comboBookingToFinish = await context.ComboBookings.FindAsync(comboBookingId);
                if (comboBookingToFinish != null)
                {
                    comboBookingToFinish.BookingStatus = 5;
                    context.ComboBookings.Update(comboBookingToFinish);
                }
                else
                {
                    throw new Exception("Combo Booking not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task ConfirmFinishBooking(int comboBookingId)
        {
            try
            {
                ComboBooking? comboBookingToFinish = await context.ComboBookings.FindAsync(comboBookingId);
                if (comboBookingToFinish != null)
                {
                    comboBookingToFinish.BookingStatus = 6;
                    context.ComboBookings.Update(comboBookingToFinish);
                }
                else
                {
                    throw new Exception("Combo Booking not found.");
                }
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
                ComboBooking? booking = await context.ComboBookings.FindAsync(bookingId);
                if (booking == null) throw new Exception("Combo Booking not found.");

                return booking.UserId == customerId;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<bool> CheckCustomerAndDetailBooking(int detailId, int customerId)
        {
            try
            {
                BookingServiceDetail? booking = await context.BookingServiceDetails.
                    Include(b => b.Booking).
                    FirstOrDefaultAsync(b => b.DetailId == detailId);
                if (booking == null) throw new Exception("Service Booking not found.");

                return booking.Booking.UserId == customerId;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<bool> CheckSupplierAndDetailBooking(int detailId, int supplierId)
        {
            try
            {
                BookingServiceDetail? booking = await context.BookingServiceDetails.
                    Include(b => b.Service).
                    FirstOrDefaultAsync(b => b.DetailId == detailId);
                if (booking == null) throw new Exception("Service Booking not found.");

                return booking.Service.SupplierId == supplierId;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}

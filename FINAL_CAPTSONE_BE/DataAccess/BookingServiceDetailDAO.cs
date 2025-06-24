using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class BookingServiceDetailDAO
    {
        private readonly WeddingWonderDbContext context;
        public BookingServiceDetailDAO(WeddingWonderDbContext context)
        {
            this.context = context;
        }
        public async Task<List<BookingServiceDetail>> GetServiceDetails()
        {
            try
            {
                List<BookingServiceDetail> serviceDetails = await context.BookingServiceDetails
                    .Include(d => d.Booking)
                    .ThenInclude(s => s.User)
                    .Include(d => d.Service)
                    .ThenInclude(s => s.Supplier)
                    .ToListAsync();

                return serviceDetails;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<BookingServiceDetail>> GetServiceDetailsByBookingId(int bookingId)
        {
            try
            {
                List<BookingServiceDetail> serviceDetails = await context.BookingServiceDetails
                    .Where(b => b.BookingId == bookingId)
                    .ToListAsync();

                return serviceDetails;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<BookingServiceDetail>> GetBySupplierId(int supplierId)
        {
            try
            {
                List<BookingServiceDetail> comboBookings = await context.BookingServiceDetails
                    .AsNoTracking()
                    .Include(d => d.Service)
                    .Where(c => c.Service.SupplierId == supplierId && c.Status != 7)
                    .Include(d => d.Booking)
                    .ThenInclude(b => b.User)
                    .ToListAsync();


                return comboBookings;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<BookingServiceDetail>> GetAcceptOfCustomer(int customerId)
        {
            try
            {
                List<BookingServiceDetail> comboBookings = await context.BookingServiceDetails
                    .AsNoTracking()
                    .Where(b => b.Booking.UserId == customerId && b.Status == 2)
                    .Include(d => d.Service)
                    .Include(d => d.Booking)
                    .ThenInclude(b => b.User)
                    .ToListAsync();

                return comboBookings;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<BookingServiceDetail>> GetFinishOfCustomer(int customerId)
        {
            try
            {
                List<BookingServiceDetail> comboBookings = await context.BookingServiceDetails
                    .AsNoTracking()
                    .Where(b => b.Booking.UserId == customerId && (b.Status == 5 || b.Status == 6))
                    .Include(d => d.Service)
                    .Include(d => d.Booking)
                    .ThenInclude(b => b.User)
                    .ToListAsync();

                return comboBookings;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<BookingServiceDetail>> GetCancelOfCustomer(int customerId)
        {
            try
            {
                List<BookingServiceDetail> comboBookings = await context.BookingServiceDetails
                    .AsNoTracking()
                    .Where(b => b.Booking.UserId == customerId && b.Status == 0)
                    .Include(d => d.Service)
                    .Include(d => d.Booking)
                    .ThenInclude(b => b.User)
                    .ToListAsync();

                return comboBookings;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<BookingServiceDetail>> GetRequestOfCustomer(int customerId)
        {
            try
            {
                List<BookingServiceDetail> comboBookings = await context.BookingServiceDetails
                    .AsNoTracking()
                    .Where(b => b.Booking.UserId == customerId && b.Status == 1)
                    .Include(d => d.Service)
                    .Include(d => d.Booking)
                    .ThenInclude(b => b.User)
                    .ToListAsync();

                return comboBookings;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<BookingServiceDetail>> GetAcceptOfSupplier(int supplierId)
        {
            try
            {
                List<BookingServiceDetail> comboBookings = await context.BookingServiceDetails
                    .AsNoTracking()
                    .Include(d => d.Service)
                    .Where(b => b.Service.SupplierId == supplierId && b.Status == 2)
                    .Include(d => d.Booking)
                    .ThenInclude(b => b.User)
                    .ToListAsync();

                return comboBookings;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<BookingServiceDetail>> GetRejectOfSupplier(int supplierId)
        {
            try
            {
                List<BookingServiceDetail> comboBookings = await context.BookingServiceDetails
                    .AsNoTracking()
                    .Include(d => d.Service)
                    .Where(b => b.Service.SupplierId == supplierId && b.Status == 3)
                    .Include(d => d.Booking)
                    .ThenInclude(b => b.User)
                    .ToListAsync();

                return comboBookings;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<BookingServiceDetail>> GetCancelOfSupplier(int supplierId)
        {
            try
            {
                List<BookingServiceDetail> comboBookings = await context.BookingServiceDetails
                    .AsNoTracking()
                    .Include(d => d.Service)
                    .Where(b => b.Service.SupplierId == supplierId && b.Status == 0)
                    .Include(d => d.Booking)
                    .ThenInclude(b => b.User)
                    .ToListAsync();

                return comboBookings;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<BookingServiceDetail>> GetRequestOfSupplier(int supplierId)
        {
            try
            {
                List<BookingServiceDetail> comboBookings = await context.BookingServiceDetails
                    .AsNoTracking()
                    .Include(d => d.Service)
                    .Where(b => b.Service.SupplierId == supplierId && b.Status == 1)
                    .Include(d => d.Booking)
                    .ThenInclude(b => b.User)
                    .ToListAsync();

                return comboBookings;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<BookingServiceDetail> GetByBookingIdAndServiceTypeOp1(int bookingId, int serviceTypeId)
        {
            try
            {
                BookingServiceDetail serviceDetail = await context.BookingServiceDetails
                    .Where(
                        b => b.BookingId == bookingId
                        && b.ServiceTypeId == serviceTypeId
                        && b.Priority == true
                        && b.Status != 0
                        && b.Status != 3)
                    .FirstOrDefaultAsync();

                return serviceDetail;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<BookingServiceDetail> GetByBookingIdAndServiceTypeOp2(int bookingId, int serviceTypeId)
        {
            try
            {
                BookingServiceDetail serviceDetail = await context.BookingServiceDetails
                    .Where(
                        b => b.BookingId == bookingId
                        && b.ServiceTypeId == serviceTypeId
                        && b.Priority == false
                        && b.Status != 0
                        && b.Status != 3)
                    .FirstOrDefaultAsync();

                return serviceDetail;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<BookingServiceDetail> GetServiceDetailById(int id)
        {
            try
            {
                BookingServiceDetail? serviceDetail = await context.BookingServiceDetails.FindAsync(id);

                return serviceDetail;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);

            }
        }
        public async Task<Boolean> CreateServiceDetail(BookingServiceDetail bookingServiceDetail)
        {
            try
            {
                bookingServiceDetail.Status = 1;
                await context.BookingServiceDetails.AddAsync(bookingServiceDetail);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<Boolean> CreateServiceDetailNotPriority(BookingServiceDetail bookingServiceDetail)
        {
            try
            {
                bookingServiceDetail.Status = 7;
                await context.BookingServiceDetails.AddAsync(bookingServiceDetail);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task UpdateServiceDetail(int id, BookingServiceDetail bookingServiceDetail)
        {
            try
            {
                context.BookingServiceDetails.Update(bookingServiceDetail);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task CancelServiceDetail(int serviceDetailId)
        {
            try
            {
                BookingServiceDetail? serviceDetail = await context.BookingServiceDetails.FindAsync(serviceDetailId);
                if (serviceDetail != null)
                {
                    if (serviceDetail.Status == 1 || serviceDetail.Status == 2)
                    {
                        serviceDetail.Status = 0;
                        context.BookingServiceDetails.Update(serviceDetail);
                    }
                }
                else
                {
                    throw new Exception("Service Detail Booking not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task AcceptServiceDetail(int serviceDetailId)
        {
            try
            {
                BookingServiceDetail? serviceDetail = await context.BookingServiceDetails.
                    Include(b => b.Booking)
                    .FirstOrDefaultAsync(b => b.DetailId == serviceDetailId);

                if (serviceDetail != null)
                {
                    if (serviceDetail.Status != 1) throw new Exception("Service Detail Booking Unacceptable.");

                    serviceDetail.Status = 2;
                    context.BookingServiceDetails.Update(serviceDetail);

                    int acceptedTypesCount = await context.BookingServiceDetails
                        .Where(d => d.BookingId == serviceDetail.BookingId && d.Status == 2)
                        .GroupBy(d => d.ServiceTypeId)
                        .CountAsync();

                    if (acceptedTypesCount != 5 && serviceDetail.Booking.TypeCombo == 1) return;
                    if (acceptedTypesCount != 3 && serviceDetail.Booking.TypeCombo == 2) return;

                    ComboBooking? booking = await context.ComboBookings.FindAsync(serviceDetail.BookingId);
                    booking.BookingStatus = 2;
                    context.ComboBookings.Update(booking);
                }
                else
                {
                    throw new Exception("Service Detail Booking not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task RejectServiceDetail(int serviceDetailId)
        {
            try
            {
                BookingServiceDetail? serviceDetail = await context.BookingServiceDetails.
                    Include(b => b.Booking)
                    .FirstOrDefaultAsync(b => b.DetailId == serviceDetailId);

                if (serviceDetail != null)
                {
                    if (serviceDetail.Status != 1) throw new Exception("Service Detail Booking Unrejectable.");

                    serviceDetail.Status = 3;
                    context.BookingServiceDetails.Update(serviceDetail);

                    if (serviceDetail.Priority)
                    {
                        BookingServiceDetail? detailsOp2 = await context.BookingServiceDetails
                            .Where(b => b.BookingId == serviceDetail.BookingId
                                    && b.ServiceTypeId == serviceDetail.ServiceTypeId
                                    && b.Priority == false
                                    && b.Status == 7)
                            .FirstOrDefaultAsync();

                        if (detailsOp2 != null)
                        {
                            detailsOp2.Status = 1;
                            context.BookingServiceDetails.Update(detailsOp2);
                        }
                    }
                    else
                    {
                        int rejectedTypesCount = await context.BookingServiceDetails
                            .GroupBy(b => b.ServiceTypeId)
                            .Where(group =>
                                group.Any(d => !d.Priority && d.Status == 3) ||
                                (group.Any(d => !d.Priority && d.Status == 0) &&
                                    group.Any(b2 =>
                                        b2.Priority &&
                                        (b2.Status == 0 || b2.Status == 3)
                                    )
                                )
                            )
                            .CountAsync();

                        if (rejectedTypesCount != 5 && serviceDetail.Booking.TypeCombo == 1) return;
                        if (rejectedTypesCount != 3 && serviceDetail.Booking.TypeCombo == 2) return;

                        ComboBooking? booking1 = await context.ComboBookings.FindAsync(serviceDetail.BookingId);
                        booking1.BookingStatus = 3;
                        context.ComboBookings.Update(booking1);
                    }
                }
                else
                {
                    throw new Exception("Service Detail Booking not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task UsingServiceDetail(int serviceDetailId)
        {
            try
            {
                BookingServiceDetail? serviceDetail = await context.BookingServiceDetails.FindAsync(serviceDetailId);
                if (serviceDetail != null)
                {
                    if (serviceDetail.Status == 2)
                    {
                        serviceDetail.Status = 4;
                        context.BookingServiceDetails.Update(serviceDetail);
                    }
                }
                else
                {
                    throw new Exception("Service Detail Booking not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task FinishServiceDetail(int serviceDetailId)
        {
            try
            {
                BookingServiceDetail? serviceDetail = await context.BookingServiceDetails.FindAsync(serviceDetailId);
                if (serviceDetail != null)
                {
                    if (serviceDetail.Status != 4 && serviceDetail.Status != 2)
                        throw new Exception("Service Detail Booking Unfinishable.");

                    serviceDetail.Status = 5;
                    context.BookingServiceDetails.Update(serviceDetail);

                    int finishedTypesCount = await context.BookingServiceDetails
                        .Where(d => d.BookingId == serviceDetail.BookingId && d.Status == 5)
                        .GroupBy(d => d.ServiceTypeId)
                        .CountAsync();

                    if (finishedTypesCount != 5 && serviceDetail.Booking.TypeCombo == 1) return;
                    if (finishedTypesCount != 3 && serviceDetail.Booking.TypeCombo == 2) return;

                    ComboBooking? booking = await context.ComboBookings.FindAsync(serviceDetail.BookingId);
                    booking.BookingStatus = 5;
                    context.ComboBookings.Update(booking);
                }
                else
                {
                    throw new Exception("Service Detail Booking not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task ConfirmFinishServiceDetail(int serviceDetailId)
        {
            try
            {
                BookingServiceDetail? serviceDetail = await context.BookingServiceDetails.FindAsync(serviceDetailId);
                if (serviceDetail != null)
                {
                    if (serviceDetail.Status != 5) throw new Exception("Service Detail Booking Unfinishable.");

                    serviceDetail.Status = 6;
                    context.BookingServiceDetails.Update(serviceDetail);
                }
                else
                {
                    throw new Exception("Service Detail Booking not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<User> GetUserByDetailBooking(int detailId)
        {
            try
            {
                User? user = await context.BookingServiceDetails
                     .AsNoTracking()
                     .Where(b => b.DetailId == detailId)
                     .Select(b => b.Booking.User)
                     .FirstOrDefaultAsync();

                return user;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}

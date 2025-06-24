using BusinessObject.DTOs;
using BusinessObject.Models;
using BusinessObject.Requests;
using Microsoft.Extensions.Configuration;
using Repository.IRepositories;

namespace Services
{
    public class BookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IInForBookingRepository _inforRepository;
        private readonly ISingleBookingRepository _singleRepository;
        private readonly IComboBookingRepository _comboRepository;
        private readonly IBookingServiceDetailRepository _detailRepository;
        private readonly IConfiguration _configuration;

        private readonly IServiceRepository _serviceRepository;

        public BookingService(
            IConfiguration configuration,
            IUnitOfWork unitOfWork,
            IInForBookingRepository inforRepository,
            ISingleBookingRepository singleRepository,
            IServiceRepository serviceRepository,
            IComboBookingRepository comboRepository,
            IBookingServiceDetailRepository detailRepository
            )
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _inforRepository = inforRepository;
            _singleRepository = singleRepository;
            _serviceRepository = serviceRepository;
            _comboRepository = comboRepository;
            _detailRepository = detailRepository;
        }
        public async Task<List<BookingToShow>> GetsHistoryForCus(int userId)
        {
            try
            {
                List<SingleBooking>? singleBookings = await _singleRepository.GetSingleBookingsByUserId(userId);
                List<ComboBooking>? comboBookings = await _comboRepository.GetByUserId(userId);
                List<BookingToShow> bookings = await ConvertSinglesForCus(singleBookings);
                bookings.AddRange(ConvertCombosForCus(comboBookings));
                bookings = bookings.OrderByDescending(b => b.CreatedAt).ToList();

                return bookings;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<BookingToShow>> GetsAcceptOfCustomer(int customerId, int type)
        {
            try
            {
                List<SingleBooking>? singleBookings = await _singleRepository.GetAcceptOfCustomer(customerId);
                List<ComboBooking>? comboBookings = await _comboRepository.GetAcceptOfCustomer(customerId);
                List<BookingToShow> bookings = new();
                switch (type)
                {
                    case 1:
                        bookings = await ConvertSinglesForCus(singleBookings);
                        bookings.AddRange(ConvertCombosForCus(comboBookings));
                        break;
                    case 2:
                        bookings = await ConvertSinglesForCus(singleBookings);
                        break;
                    case 3:
                        bookings = ConvertCombosForCus(comboBookings);
                        break;
                    default:
                        throw new Exception("Type incorrect");
                }
                bookings = bookings.OrderByDescending(b => b.CreatedAt).ToList();

                return bookings;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<BookingToShow>> GetsFinishOfCustomer(int customerId, int type)
        {
            try
            {
                List<SingleBooking>? singleBookings = await _singleRepository.GetFinishOfCustomer(customerId);
                List<ComboBooking>? comboBookings = await _comboRepository.GetFinishOfCustomer(customerId);
                List<BookingToShow> bookings = new();
                switch (type)
                {
                    case 1:
                        bookings = await ConvertSinglesForCus(singleBookings);
                        bookings.AddRange(ConvertCombosForCus(comboBookings));
                        break;
                    case 2:
                        bookings = await ConvertSinglesForCus(singleBookings);
                        break;
                    case 3:
                        bookings = ConvertCombosForCus(comboBookings);
                        break;
                    default:
                        throw new Exception("Type incorrect");
                }
                bookings = bookings.OrderByDescending(b => b.CreatedAt).ToList();

                return bookings;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<BookingToShow>> GetsCancelOfCustomer(int customerId, int type)
        {
            try
            {
                List<SingleBooking>? singleBookings = await _singleRepository.GetCancelOfCustomer(customerId);
                List<ComboBooking>? comboBookings = await _comboRepository.GetCancelOfCustomer(customerId);
                List<BookingToShow> bookings = new();
                switch (type)
                {
                    case 1:
                        bookings = await ConvertSinglesForCus(singleBookings);
                        bookings.AddRange(ConvertCombosForCus(comboBookings));
                        break;
                    case 2:
                        bookings = await ConvertSinglesForCus(singleBookings);
                        break;
                    case 3:
                        bookings = ConvertCombosForCus(comboBookings);
                        break;
                    default:
                        throw new Exception("Type incorrect");
                }
                bookings = bookings.OrderByDescending(b => b.CreatedAt).ToList();

                return bookings;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<BookingToShow>> GetsRequestOfCustomer(int customerId, int type)
        {
            try
            {
                List<SingleBooking>? singleBookings = await _singleRepository.GetRequestOfCustomer(customerId);
                List<ComboBooking>? comboBookings = await _comboRepository.GetRequestOfCustomer(customerId);
                List<BookingToShow> bookings = new();
                switch (type)
                {
                    case 1:
                        bookings = await ConvertSinglesForCus(singleBookings);
                        bookings.AddRange(ConvertCombosForCus(comboBookings));
                        break;
                    case 2:
                        bookings = await ConvertSinglesForCus(singleBookings);
                        break;
                    case 3:
                        bookings = ConvertCombosForCus(comboBookings);
                        break;
                    default:
                        throw new Exception("Type incorrect");
                }
                bookings = bookings.OrderByDescending(b => b.CreatedAt).ToList();

                return bookings;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<int> GetsCountAcceptOfCustomer(int customerId, int type)
        {
            try
            {
                List<SingleBooking>? singleBookings = await _singleRepository.GetAcceptOfCustomer(customerId);
                List<ComboBooking>? comboBookings = await _comboRepository.GetAcceptOfCustomer(customerId);

                int count = 0;

                switch (type)
                {
                    case 1:
                        count = singleBookings.Count + comboBookings.Count;
                        break;
                    case 2:
                        count = singleBookings.Count;
                        break;
                    case 3:
                        count = comboBookings.Count;
                        break;
                    default:
                        throw new Exception("Type incorrect");
                }

                return count;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<int> GetsCountFinishOfCustomer(int customerId, int type)
        {
            try
            {
                List<SingleBooking>? singleBookings = await _singleRepository.GetFinishOfCustomer(customerId);
                List<ComboBooking>? comboBookings = await _comboRepository.GetFinishOfCustomer(customerId);

                int count = 0;

                switch (type)
                {
                    case 1:
                        count = singleBookings.Count + comboBookings.Count;
                        break;
                    case 2:
                        count = singleBookings.Count;
                        break;
                    case 3:
                        count = comboBookings.Count;
                        break;
                    default:
                        throw new Exception("Type incorrect");
                }

                return count;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> GetsCountCancelOfCustomer(int customerId, int type)
        {
            try
            {
                List<SingleBooking>? singleBookings = await _singleRepository.GetCancelOfCustomer(customerId);
                List<ComboBooking>? comboBookings = await _comboRepository.GetCancelOfCustomer(customerId);

                int count = 0;

                switch (type)
                {
                    case 1:
                        count = singleBookings.Count + comboBookings.Count;
                        break;
                    case 2:
                        count = singleBookings.Count;
                        break;
                    case 3:
                        count = comboBookings.Count;
                        break;
                    default:
                        throw new Exception("Type incorrect");
                }

                return count;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> GetsCountRequestOfCustomer(int customerId, int type)
        {
            try
            {
                List<SingleBooking>? singleBookings = await _singleRepository.GetRequestOfCustomer(customerId);
                List<ComboBooking>? comboBookings = await _comboRepository.GetRequestOfCustomer(customerId);

                int count = 0;

                switch (type)
                {
                    case 1:
                        count = singleBookings.Count + comboBookings.Count;
                        break;
                    case 2:
                        count = singleBookings.Count;
                        break;
                    case 3:
                        count = comboBookings.Count;
                        break;
                    default:
                        throw new Exception("Type incorrect");
                }

                return count;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<BookingToShow>> GetsHistoryForSupplier(int supplierId)
        {
            try
            {
                List<SingleBooking>? singleBookings = await _singleRepository.GetSingleBookingsBySupplierId(supplierId);
                List<BookingServiceDetail>? comboBookings = await _detailRepository.GetBySupplierId(supplierId);
                List<BookingToShow> bookings = await ConvertSinglesForSup(singleBookings);
                bookings.AddRange(await ConvertCombosForSup(comboBookings));
                bookings = bookings.OrderByDescending(b => b.CreatedAt).ToList();

                return bookings;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<BookingToShow>> GetsAcceptOfSupplier(int supplierId, int type)
        {
            try
            {
                List<SingleBooking>? singleBookings = await _singleRepository.GetAcceptOfSupplier(supplierId);
                List<BookingServiceDetail>? comboBookings = await _detailRepository.GetAcceptOfSupplier(supplierId);
                List<BookingToShow> bookings = new();
                switch (type)
                {
                    case 1:
                        bookings = await ConvertSinglesForSup(singleBookings);
                        bookings.AddRange(await ConvertCombosForSup(comboBookings));
                        break;
                    case 2:
                        bookings = await ConvertSinglesForSup(singleBookings);
                        break;
                    case 3:
                        bookings = await ConvertCombosForSup(comboBookings);
                        break;
                    default:
                        throw new Exception("Type incorrect");
                }
                bookings = bookings.OrderByDescending(b => b.CreatedAt).ToList();

                return bookings;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<BookingToShow>> GetsRejectOfSupplier(int supplierId, int type)
        {
            try
            {
                List<SingleBooking>? singleBookings = await _singleRepository.GetRejectOfSupplier(supplierId);
                List<BookingServiceDetail>? comboBookings = await _detailRepository.GetRejectOfSupplier(supplierId);
                List<BookingToShow> bookings = new();
                switch (type)
                {
                    case 1:
                        bookings = await ConvertSinglesForSup(singleBookings);
                        bookings.AddRange(await ConvertCombosForSup(comboBookings));
                        break;
                    case 2:
                        bookings = await ConvertSinglesForSup(singleBookings);
                        break;
                    case 3:
                        bookings = await ConvertCombosForSup(comboBookings);
                        break;
                    default:
                        throw new Exception("Type incorrect");
                }
                bookings = bookings.OrderByDescending(b => b.CreatedAt).ToList();

                return bookings;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<BookingToShow>> GetsCancelOfSupplier(int supplierId, int type)
        {
            try
            {
                List<SingleBooking>? singleBookings = await _singleRepository.GetCancelOfSupplier(supplierId);
                List<BookingServiceDetail>? comboBookings = await _detailRepository.GetCancelOfSupplier(supplierId);
                List<BookingToShow> bookings = new();
                switch (type)
                {
                    case 1:
                        bookings = await ConvertSinglesForSup(singleBookings);
                        bookings.AddRange(await ConvertCombosForSup(comboBookings));
                        break;
                    case 2:
                        bookings = await ConvertSinglesForSup(singleBookings);
                        break;
                    case 3:
                        bookings = await ConvertCombosForSup(comboBookings);
                        break;
                    default:
                        throw new Exception("Type incorrect");
                }
                bookings = bookings.OrderByDescending(b => b.CreatedAt).ToList();

                return bookings;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<BookingToShow>> GetsRequestOfSupplier(int supplierId, int type)
        {
            try
            {
                List<SingleBooking>? singleBookings = await _singleRepository.GetRequestOfSupplier(supplierId);
                List<BookingServiceDetail>? comboBookings = await _detailRepository.GetRequestOfSupplier(supplierId);
                List<BookingToShow> bookings = new();
                switch (type)
                {
                    case 1:
                        bookings = await ConvertSinglesForSup(singleBookings);
                        bookings.AddRange(await ConvertCombosForSup(comboBookings));
                        break;
                    case 2:
                        bookings = await ConvertSinglesForSup(singleBookings);
                        break;
                    case 3:
                        bookings = await ConvertCombosForSup(comboBookings);
                        break;
                    default:
                        throw new Exception("Type incorrect");
                }
                bookings = bookings.OrderByDescending(b => b.CreatedAt).ToList();

                return bookings;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        //--------------------------------------------
        private async Task<List<BookingToShow>> ConvertSinglesForCus(List<SingleBooking> bookings)
        {
            try
            {
                List<BookingToShow> bookingToShowList = new();

                foreach (SingleBooking booking in bookings)
                {
                    BookingToShow bookingToShow = await ConvertSingleForCus(booking);
                    bookingToShowList.Add(bookingToShow);
                }

                return bookingToShowList.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        private List<BookingToShow> ConvertCombosForCus(List<ComboBooking> bookings)
        {
            try
            {
                List<BookingToShow> bookingToShowList = bookings.Select(b => ConvertComboForCus(b)).ToList();
                return bookingToShowList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        private async Task<BookingToShow> ConvertSingleForCus(SingleBooking booking)
        {
            Service service = await _serviceRepository.GetAsyncById(booking.BookingId);
            return new BookingToShow
            {
                BookingId = booking.BookingId,
                ServiceId = booking.ServiceId,
                ServiceName = service.ServiceName,
                CreatedAt = booking.CreatedAt,
                DateOfUse = booking.DateOfUse,
                BookingStatus = booking.BookingStatus,
                TypeBooking = 1
            };
        }
        private BookingToShow ConvertComboForCus(ComboBooking booking)
        {
            BookingToShow? bookingShow = new()
            {
                BookingId = booking.BookingId,
                CreatedAt = booking.CreatedAt,
                BookingStatus = booking.BookingStatus,
                DateOfUse = booking.ExpectedWeddingDate
            };
            if (booking.TypeCombo == 1)
            {
                bookingShow.ServiceName = "Combo Wedding Full Service";
                bookingShow.TypeBooking = 2;
            }
            else
            {
                bookingShow.ServiceName = "Combo Wedding Partial Service";
                bookingShow.TypeBooking = 3;
            }

            return bookingShow;
        }
        private async Task<List<BookingToShow>> ConvertSinglesForSup(List<SingleBooking> bookings)
        {
            try
            {
                List<BookingToShow> bookingToShowList = new();

                foreach (SingleBooking booking in bookings)
                {
                    BookingToShow bookingToShow = await ConvertSingleForSup(booking);
                    bookingToShowList.Add(bookingToShow);
                }

                return bookingToShowList;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        private async Task<List<BookingToShow>> ConvertCombosForSup(List<BookingServiceDetail> bookings)
        {
            try
            {
                List<BookingToShow> bookingToShowList = new();

                foreach (BookingServiceDetail booking in bookings)
                {
                    BookingToShow bookingToShow = await ConvertComboForSup(booking);
                    bookingToShowList.Add(bookingToShow);
                }

                return bookingToShowList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        private async Task<BookingToShow> ConvertSingleForSup(SingleBooking booking)
        {
            Service service = await _serviceRepository.GetAsyncById(booking.BookingId);
            return new BookingToShow
            {
                BookingId = booking.BookingId,
                FullName = booking.User.FullName,
                UserImage = booking.User.UserImage,
                ServiceName = service.ServiceName,
                Infor = new InforBookingDTO
                {
                    FullName = booking.Infor.FullName,
                    PhoneNumber = booking.Infor.PhoneNumber,
                    City = booking.Infor.City,
                    District = booking.Infor.District,
                    Ward = booking.Infor.Ward,
                    Address = booking.Infor.Address
                },
                ServiceId = booking.ServiceId,
                ServiceTypeId = booking.ServiceTypeId,
                PackageId = booking.PackageId,
                DateOfUse = booking.DateOfUse,
                CreatedAt = booking.CreatedAt,
                BookingStatus = booking.BookingStatus,
                TypeBooking = 1
            };
        }
        private async Task<BookingToShow> ConvertComboForSup(BookingServiceDetail booking)
        {
            BookingToShow bookingToShow = new()
            {
                BookingId = booking.DetailId,
                UserImage = booking.Booking.User.UserImage,
                ServiceName = booking.Service.ServiceName,
                ServiceId = booking.ServiceId,
                ServiceTypeId = booking.ServiceTypeId,
                PackageId = booking.PackageId,
                DateOfUse = booking.Booking.ExpectedWeddingDate,
                Note = booking.Note,
                TotalPrice = booking.TotalPrice,
                CreatedAt = booking.Booking.CreatedAt,
                BookingStatus = booking.Status
            };

            bookingToShow.TypeBooking = booking.Booking.TypeCombo + 1;
            InforBooking groom = await _inforRepository.GetAsyncById(booking.Booking.InforGroomId);
            InforBooking bride = await _inforRepository.GetAsyncById(booking.Booking.InforBrideId);
            bookingToShow.Groom = await ComboBookingService.ConvetInforBooking(groom);
            bookingToShow.Bride = await ComboBookingService.ConvetInforBooking(bride);

            return bookingToShow;
        }

        public async Task<int> GetsCountAcceptOfSupplier(int supplierId, int type)
        {
            try
            {
                List<SingleBooking>? singleBookings = await _singleRepository.GetAcceptOfSupplier(supplierId);
                List<BookingServiceDetail>? comboBookings = await _detailRepository.GetAcceptOfSupplier(supplierId);

                int count = 0;

                switch (type)
                {
                    case 1:
                        count = singleBookings.Count + comboBookings.Count;
                        break;
                    case 2:
                        count = singleBookings.Count;
                        break;
                    case 3:
                        count = comboBookings.Count;
                        break;
                    default:
                        throw new Exception("Type incorrect");
                }

                return count;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> GetsCountRejectOfSupplier(int supplierId, int type)
        {
            try
            {
                List<SingleBooking>? singleBookings = await _singleRepository.GetRejectOfSupplier(supplierId);
                List<BookingServiceDetail>? comboBookings = await _detailRepository.GetRejectOfSupplier(supplierId);

                int count = 0;

                switch (type)
                {
                    case 1:
                        count = singleBookings.Count + comboBookings.Count;
                        break;
                    case 2:
                        count = singleBookings.Count;
                        break;
                    case 3:
                        count = comboBookings.Count;
                        break;
                    default:
                        throw new Exception("Type incorrect");
                }

                return count;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> GetsCountCancelOfSupplier(int supplierId, int type)
        {
            try
            {
                List<SingleBooking>? singleBookings = await _singleRepository.GetCancelOfSupplier(supplierId);
                List<BookingServiceDetail>? comboBookings = await _detailRepository.GetCancelOfSupplier(supplierId);

                int count = 0;

                switch (type)
                {
                    case 1:
                        count = singleBookings.Count + comboBookings.Count;
                        break;
                    case 2:
                        count = singleBookings.Count;
                        break;
                    case 3:
                        count = comboBookings.Count;
                        break;
                    default:
                        throw new Exception("Type incorrect");
                }

                return count;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> GetsCountRequestOfSupplier(int supplierId, int type)
        {
            try
            {
                List<SingleBooking>? singleBookings = await _singleRepository.GetRequestOfSupplier(supplierId);
                List<BookingServiceDetail>? comboBookings = await _detailRepository.GetRequestOfSupplier(supplierId);

                int count = 0;

                switch (type)
                {
                    case 1:
                        count = singleBookings.Count + comboBookings.Count;
                        break;
                    case 2:
                        count = singleBookings.Count;
                        break;
                    case 3:
                        count = comboBookings.Count;
                        break;
                    default:
                        throw new Exception("Type incorrect");
                }

                return count;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}

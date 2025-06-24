using BusinessObject.Contracts.IntergrationEvents;
using BusinessObject.DTOs;
using BusinessObject.Models;
using BusinessObject.Requests;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Repository.IRepositories;

namespace Services
{
    public class SingleBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IInForBookingRepository _inforRepository;
        private readonly ISingleBookingRepository _bookingRepository;
        private readonly IConfiguration _configuration;
        private readonly IServiceRepository _serviceRepository;
        private readonly IPhotographPackageRepository _photographRepository;
        private readonly IEventPackageRepository _eventRepository;
        private readonly IMakeUpPackageRepository _makeUpRepository;
        private readonly IInvitationPackageRepository _invitationPackage;
        private readonly IOutfitRepository _outfitRepository;
        private readonly ICateringRepository _cateringRepository;
        private readonly IPublishEndpoint _publishEndpoint;

        public SingleBookingService(
            IConfiguration configuration,
            IUnitOfWork unitOfWork,
            IInForBookingRepository inforRepository,
            ISingleBookingRepository bookingRepository,
            IServiceRepository serviceRepository,
            IEventPackageRepository eventRepository,
            IMakeUpPackageRepository makeUpRepository,
            IInvitationPackageRepository invitationPackage,
            IOutfitRepository outfitRepository,
            IPhotographPackageRepository photographRepository,
            ICateringRepository cateringRepository,
            IPublishEndpoint publishEndpoint
            )
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _inforRepository = inforRepository;
            _bookingRepository = bookingRepository;
            _serviceRepository = serviceRepository;
            _outfitRepository = outfitRepository;
            _cateringRepository = cateringRepository;
            _eventRepository = eventRepository;
            _makeUpRepository = makeUpRepository;
            _eventRepository = eventRepository;
            _photographRepository = photographRepository;
            _invitationPackage = invitationPackage;
            _publishEndpoint = publishEndpoint;
        }
        // Create
        public async Task CreateBooking(int userId, SingleBookingDTO request)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                if (request.DateOfUse < DateTime.Now.Date) throw new Exception("Invalid date of use service ");

                Service service = await _serviceRepository.GetAsyncById(request.ServiceId);
                await GetNamePackage(request.PackageId, service.ServiceTypeId);

                InforBooking infor = new()
                {
                    FullName = request.FullName,
                    PhoneNumber = request.PhoneNumber,
                    City = request.City,
                    District = request.District,
                    Ward = request.Ward,
                    Address = request.Address
                };

                await _inforRepository.CreateAsync(infor);
                await _unitOfWork.CommitAsync();

                SingleBooking booking = new()
                {
                    DateOfUse = request.DateOfUse,
                    CreatedAt = DateTime.Now,
                    InforId = infor.InforId,
                    BookingStatus = 1,
                    NumberOfUses = request.NumberOfUses,
                    PackageId = request.PackageId,
                    ServiceId = request.ServiceId,
                    ServiceTypeId = service.ServiceTypeId,
                    UserId = userId,
                };

                await _bookingRepository.CreateAsync(booking);

                EmailNotificationEvent emailEvent = new()
                {
                    Id = Guid.NewGuid(),
                    ReceiverEmail = service.Supplier.Email,
                    ReceiverName = service.Supplier.FullName,
                    Subject = "Booking Acceptance Notification",
                    Body = $"Dear {service.Supplier.FullName},<br><br>" +
                   $"Your store {service.ServiceName} has a booking for the customer {request.FullName} in our system.<br><br>" +
                   "To track the status of your services and view further details, please log in to your account on our system http://localhost:3000/.<br><br>" +
                   "If you have any questions or need further assistance, feel free to contact our support team.<br><br>" +
                   "Best regards,<br>" +
                   "Wedding Wonder Team",
                    TimeStamp = DateTimeOffset.Now
                };

                await _publishEndpoint.Publish(emailEvent);
                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception(ex.Message, ex);
            }
        }
        // Read
        public async Task<List<SingleBookingToShow>> GetsAllOfSupplierId(int supplierId)
        {
            try
            {
                List<SingleBooking>? bookings = await _bookingRepository.GetSingleBookingsBySupplierId(supplierId);

                return await ConvertToBookingShow(bookings);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<SingleBookingToShow>> GetsAllOfUser(int userId)
        {
            try
            {
                List<SingleBooking>? bookings = await _bookingRepository.GetSingleBookingsByUserId(userId);

                return await ConvertToBookingShow(bookings);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<SingleBookingToShow>> GetsAcceptOfSupplier(int supplierId)
        {
            try
            {
                List<SingleBooking>? bookings = await _bookingRepository.GetAcceptOfSupplier(supplierId);

                return await ConvertToBookingShow(bookings);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<SingleBookingToShow>> GetsRejectOfSupplier(int supplierId)
        {
            try
            {
                List<SingleBooking>? bookings = await _bookingRepository.GetRejectOfSupplier(supplierId);

                return await ConvertToBookingShow(bookings);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<SingleBookingToShow>> GetsCancelOfSupplier(int supplierId)
        {
            try
            {
                List<SingleBooking>? bookings = await _bookingRepository.GetCancelOfSupplier(supplierId);

                return await ConvertToBookingShow(bookings);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<SingleBookingToShow>> GetsRequestBooking(int supplierId)
        {
            try
            {
                List<SingleBooking>? bookings = await _bookingRepository.GetRequestOfSupplier(supplierId);

                return await ConvertToBookingShow(bookings);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<SingleBookingToShow> GetById(int serviceId)
        {
            try
            {
                SingleBooking? booking = await _bookingRepository.GetAsyncById(serviceId);

                return ConvertSingleBooking(booking);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<int> GetCountStatusBySupplierId(int id, int status)
        {
            try
            {
                return await _bookingRepository.CountStatusBookingBySupplierId(id, status);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<int> GetCountStatusByServiceId(int id, int status)
        {
            try
            {
                return await _bookingRepository.CountStatusBookingByServiceId(id, status);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<int> GetCountStatusByUserId(int id, int status)
        {
            try
            {
                return await _bookingRepository.CountStatusBookingByUserId(id, status);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        // Update
        public async Task UpdateBooking(int id, SingleBookingDTO request)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                SingleBooking booking = await _bookingRepository.GetAsyncById(id);

                if (booking.BookingStatus != 1 && booking.BookingStatus != 2)
                    throw new Exception("This booking cannot be edited.");

                InforBooking infor = new()
                {
                    City = request.City,
                    District = request.District,
                    Ward = request.Ward,
                    Address = request.Address,
                    FullName = request.FullName,
                    PhoneNumber = request.PhoneNumber,
                    InforId = booking.InforId
                };

                await _inforRepository.UpdateAsync(id, infor);

                booking.DateOfUse = request.DateOfUse;
                booking.PackageId = request.PackageId;
                booking.NumberOfUses = request.NumberOfUses;

                await _bookingRepository.UpdateAsync(id, booking);

                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task UpdateBookingForSup(int id, SingleBookingDTO request)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                SingleBooking booking = await _bookingRepository.GetAsyncById(id);

                if (booking.BookingStatus >= 1 && booking.BookingStatus <= 3)
                    throw new Exception("This booking cannot be edited.");

                InforBooking infor = new()
                {
                    City = request.City,
                    District = request.District,
                    Ward = request.Ward,
                    Address = request.Address,
                    FullName = request.FullName,
                    PhoneNumber = request.PhoneNumber,
                    InforId = booking.InforId
                };

                await _inforRepository.UpdateAsync(id, infor);

                booking.DateOfUse = request.DateOfUse;
                booking.PackageId = request.PackageId;
                booking.NumberOfUses = request.NumberOfUses;

                await _bookingRepository.UpdateAsync(id, booking);

                EmailNotificationEvent emailEvent = new()
                {
                    Id = Guid.NewGuid(),
                    ReceiverEmail = booking.User.Email,
                    ReceiverName = booking.User.FullName,
                    Subject = "Booking Update Notification",
                    Body = $"Dear {booking.User.FullName},<br><br>" +
                   "Your booking has been updated by the supplier. Please log in to our system to track the changes and view further details http://localhost:3000/.<br><br>" +
                   "If you have any questions or need further assistance, feel free to contact our support team.<br><br>" +
                   "Best regards,<br>" +
                   "Wedding Wonder Team",
                    TimeStamp = DateTimeOffset.Now
                };

                await _publishEndpoint.Publish(emailEvent);
                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task AcceptBooking(int id)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                await _bookingRepository.AcceptSingleBooking(id);
                SingleBooking booking = await _bookingRepository.GetAsyncById(id);

                EmailNotificationEvent emailEvent = new()
                {
                    Id = Guid.NewGuid(),
                    ReceiverEmail = booking.User.Email,
                    ReceiverName = booking.User.FullName,
                    Subject = "Booking Acceptance Notification",
                    Body = $"Dear {booking.User.FullName},<br><br>" +
                  $"Your booking for {booking.Service.ServiceName} store has been accepted by the supplier.<br><br>" +
                  "To track the status of your services and view further details, please log in to your account on our system http://localhost:3000/.<br><br>" +
                  "If you have any questions or need further assistance, feel free to contact our support team.<br><br>" +
                  "Best regards,<br>" +
                  "Wedding Wonder Team",
                    TimeStamp = DateTimeOffset.Now
                };

                await _publishEndpoint.Publish(emailEvent);
                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task RejectBooking(int id)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                await _bookingRepository.RejectSingleBooking(id);
                SingleBooking booking = await _bookingRepository.GetAsyncById(id);

                EmailNotificationEvent emailEvent = new()
                {
                    Id = Guid.NewGuid(),
                    ReceiverEmail = booking.User.Email,
                    ReceiverName = booking.User.FullName,
                    Subject = "Booking Rejection Notification",
                    Body = $"Dear {booking.User.FullName},<br><br>" +
                  $"We regret to inform you that your booking at {booking.Service.ServiceName} store has been rejected by the supplier.<br><br>" +
                  "To track the status of your services and view further details, please log in to your account on our system http://localhost:3000/.<br><br>" +
                  "If you have any questions or need further assistance, feel free to contact our support team.<br><br>" +
                  "Best regards,<br>" +
                  "Wedding Wonder Team",
                    TimeStamp = DateTimeOffset.Now
                };

                await _publishEndpoint.Publish(emailEvent);

                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task CancelBooking(int id)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                await _bookingRepository.CancelSingleBooking(id);

                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task UsingBooking(int id)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                await _bookingRepository.UsingSingleBooking(id);

                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task FinishBooking(int id)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                await _bookingRepository.FinishSingleBooking(id);

                SingleBooking booking = await _bookingRepository.GetAsyncById(id);

                EmailNotificationEvent emailEvent = new()
                {
                    Id = Guid.NewGuid(),
                    ReceiverEmail = booking.User.Email,
                    ReceiverName = booking.User.FullName,
                    Subject = "Booking Completion Notification",
                    Body = $"Dear {booking.User.FullName},<br><br>" +
                   $"Your booking at {booking.Service.ServiceName} store is complete. Please confirm and leave your comments and price rating about the service used.<br><br>" +
                   "Your sharing will help our services better in the future. Thank you very much for using our services.<br><br>" +
                   "To track the status of your services and view further details, please log in to your account on our system http://localhost:3000/.<br><br>" +
                   "If you have any questions or need further assistance, feel free to contact our support team.<br><br>" +
                   "Best regards,<br>" +
                   "Wedding Wonder Team",
                    TimeStamp = DateTimeOffset.Now
                };

                await _publishEndpoint.Publish(emailEvent);

                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task ConFirmFinishBooking(int id)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                await _bookingRepository.ConfirmSingleBooking(id);

                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception(ex.Message, ex);
            }
        }
        // Delete

        //Other
        public async Task<bool> CheckSupplierAndService(int supplierId, int serviceId)
        {
            try
            {
                bool status = await _serviceRepository.CheckSupplierAndService(supplierId, serviceId);
                return status;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<bool> CheckSupplierAndBooking(int supplierId, int bookingId)
        {
            try
            {
                bool status = await _bookingRepository.CheckSupplierAndBooking(bookingId, supplierId);
                return status;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<bool> CheckCustomerAndBooking(int customerId, int bookingId)
        {
            try
            {
                bool status = await _bookingRepository.CheckCustomerAndBooking(bookingId, customerId);
                return status;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        private async Task<string> GetNamePackage(int packageId, int serviceType)
        {
            try
            {
                string name;
                switch (serviceType)
                {
                    case (int)ServiceType.Makeup:
                        MakeUpPackage makeUp = await _makeUpRepository.GetAsyncById(packageId);
                        name = makeUp.PackageName;
                        break;

                    case (int)ServiceType.Clothes:
                        Outfit outfit = await _outfitRepository.GetAsyncById(packageId);
                        name = outfit.OutfitName;
                        break;

                    case (int)ServiceType.Restaurant:
                        Catering catering = await _cateringRepository.GetAsyncById(packageId);
                        name = catering.StyleName;
                        break;

                    case (int)ServiceType.Photograph:
                        PhotographPackage photo = await _photographRepository.GetAsyncById(packageId);
                        name = photo.PackageName;
                        break;

                    case (int)ServiceType.Invitation:
                        InvitationPackage invitation = await _invitationPackage.GetAsyncById(packageId);
                        name = invitation.PackageName;
                        break;

                    case (int)ServiceType.EventOrganizer:
                        EventPackage even = await _eventRepository.GetAsyncById(packageId);
                        name = even.PackageName;
                        break;
                    default:
                        throw new Exception("Service type invalid");
                }
                return name;
            }
            catch (Exception ex)
            {
                throw new Exception("Package service does not exist");
            }
        }
        private async Task<List<SingleBookingToShow>> ConvertToBookingShow(List<SingleBooking> bookings)
        {
            try
            {
                return await Task.Run(() =>
                    bookings.Select(b => ConvertSingleBooking(b)).ToList()
                );
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        private SingleBookingToShow ConvertSingleBooking(SingleBooking booking)
        {
            return new SingleBookingToShow
            {
                BookingId = booking.BookingId,
                FullName = booking.User.FullName,
                UserImage = booking.User.UserImage,
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
                BookingStatus = booking.BookingStatus
            };
        }
    }
}
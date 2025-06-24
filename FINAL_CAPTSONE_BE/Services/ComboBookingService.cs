using BusinessObject.Contracts.IntergrationEvents;
using BusinessObject.DTOs;
using BusinessObject.Models;
using BusinessObject.Requests;
using BusinessObject.Requests.ComboBooking;
using MassTransit;
using Microsoft.Extensions.Configuration;
using MimeKit;
using Repository.IRepositories;
using Services.Email;
using Message = Services.Email.Message;

namespace Services
{
    public class ComboBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IInForBookingRepository _inforRepository;
        private readonly IComboBookingRepository _bookingRepository;
        private readonly IConfiguration _configuration;

        private readonly IServiceRepository _serviceRepository;
        private readonly IPhotographPackageRepository _photographRepository;
        private readonly IEventPackageRepository _eventRepository;
        private readonly IMakeUpPackageRepository _makeUpRepository;
        private readonly IInvitationPackageRepository _invitationPackage;
        private readonly IOutfitRepository _outfitRepository;
        private readonly ICateringRepository _cateringRepository;
        private readonly IBookingServiceDetailRepository _detailBookingRepository;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IEmailService _emailService;
        private readonly IVoucherAdminRepository _voucherRepository;

        public ComboBookingService(
            IConfiguration configuration,
            IUnitOfWork unitOfWork,
            IInForBookingRepository inforRepository,
            IComboBookingRepository bookingRepository,
            IServiceRepository serviceRepository,
            IEventPackageRepository eventRepository,
            IMakeUpPackageRepository makeUpRepository,
            IInvitationPackageRepository invitationPackage,
            IOutfitRepository outfitRepository,
            IPhotographPackageRepository photographRepository,
            ICateringRepository cateringRepository,
            IBookingServiceDetailRepository detailBookingRepository,
            IEmailService emailService,
            IPublishEndpoint publishEndpoint,
            IVoucherAdminRepository voucherRepository
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
            _detailBookingRepository = detailBookingRepository;
            _emailService = emailService;
            _publishEndpoint = publishEndpoint;
            _voucherRepository = voucherRepository;
        }

        //Create
        public async Task CreateBookingFull(int userId, ComboBookingDTO request)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                InforBooking groom = new()
                {
                    FullName = request.Groom.FullName,
                    PhoneNumber = request.Groom.PhoneNumber,
                    City = request.Groom.City,
                    District = request.Groom.District,
                    Ward = request.Groom.Ward,
                    Address = request.Groom.Address,
                    DateOfBirth = request.Groom.DateOfBirth
                };
                InforBooking bride = new()
                {
                    FullName = request.Bride.FullName,
                    PhoneNumber = request.Bride.PhoneNumber,
                    City = request.Bride.City,
                    District = request.Bride.District,
                    Ward = request.Bride.Ward,
                    Address = request.Bride.Address,
                    DateOfBirth = request.Bride.DateOfBirth
                };

                await _inforRepository.CreateAsync(groom);
                await _inforRepository.CreateAsync(bride);
                await _unitOfWork.CommitAsync();

                ComboBooking booking = new()
                {
                    ExpectedWeddingDate = request.ExpectedWeddingDate,
                    CreatedAt = DateTime.Now,
                    UserId = userId,
                    InforBrideId = bride.InforId,
                    InforGroomId = groom.InforId,
                    VoucherId = request.VoucherId,
                    TypeCombo = 1,
                    BookingStatus = 1
                };
                await _bookingRepository.CreateAsync(booking);
                await _unitOfWork.CommitAsync();

                VoucherAdmin? voucher = await _voucherRepository.GetAsyncById(booking.VoucherId ?? 0);

                await InsertBookingDetail(voucher, request.MakeUpOp1, booking.BookingId, 1, true);
                await InsertBookingDetail(voucher, request.MakeUpOp2, booking.BookingId, 1, false);
                await InsertBookingDetail(voucher, request.ClothesOp1, booking.BookingId, 2, true);
                await InsertBookingDetail(voucher, request.ClothesOp2, booking.BookingId, 2, false);
                await InsertBookingDetail(voucher, request.RestaurantOp1, booking.BookingId, 3, true);
                await InsertBookingDetail(voucher, request.RestaurantOp2, booking.BookingId, 3, false);
                await InsertBookingDetail(voucher, request.PhotoOp1, booking.BookingId, 4, true);
                await InsertBookingDetail(voucher, request.PhotoOp2, booking.BookingId, 4, false);
                await InsertBookingDetail(voucher, request.InvitationOp1, booking.BookingId, 5, true);
                await InsertBookingDetail(voucher, request.InvitationOp2, booking.BookingId, 5, false);
                await InsertBookingDetail(voucher, request.EventOp1, booking.BookingId, 6, true);
                await InsertBookingDetail(voucher, request.EventOp2, booking.BookingId, 6, false);

                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();

                _ = SendEmailForSupplier(groom.FullName, bride.FullName, request.MakeUpOp1.ServiceId);
                _ = SendEmailForSupplier(groom.FullName, bride.FullName, request.ClothesOp1.ServiceId);
                _ = SendEmailForSupplier(groom.FullName, bride.FullName, request.RestaurantOp1.ServiceId);
                _ = SendEmailForSupplier(groom.FullName, bride.FullName, request.PhotoOp1.ServiceId);
                _ = SendEmailForSupplier(groom.FullName, bride.FullName, request.InvitationOp1.ServiceId);
                _ = SendEmailForSupplier(groom.FullName, bride.FullName, request.EventOp1.ServiceId);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task CreateBookingPartial(int userId, ComboBookingDTO request)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                InforBooking groom = new()
                {
                    FullName = request.Groom.FullName,
                    PhoneNumber = request.Groom.PhoneNumber,
                    City = request.Groom.City,
                    District = request.Groom.District,
                    Ward = request.Groom.Ward,
                    Address = request.Groom.Address,
                    DateOfBirth = request.Groom.DateOfBirth
                };
                InforBooking bride = new()
                {
                    FullName = request.Groom.FullName,
                    PhoneNumber = request.Groom.PhoneNumber,
                    City = request.Groom.City,
                    District = request.Groom.District,
                    Ward = request.Groom.Ward,
                    Address = request.Groom.Address,
                    DateOfBirth = request.Groom.DateOfBirth
                };

                await _inforRepository.CreateAsync(groom);
                await _inforRepository.CreateAsync(bride);
                await _unitOfWork.CommitAsync();

                ComboBooking booking = new()
                {
                    ExpectedWeddingDate = request.ExpectedWeddingDate,
                    CreatedAt = DateTime.Now,
                    UserId = userId,
                    InforBrideId = bride.InforId,
                    InforGroomId = groom.InforId,
                    VoucherId = request.VoucherId,
                    TypeCombo = 2,
                    BookingStatus = 1
                };
                await _bookingRepository.CreateAsync(booking);
                await _unitOfWork.CommitAsync();

                VoucherAdmin? voucher = await _voucherRepository.GetAsyncById(booking.VoucherId ?? 0);

                await InsertBookingDetail(voucher, request.ClothesOp1, booking.BookingId, 2, true);
                await InsertBookingDetail(voucher, request.ClothesOp2, booking.BookingId, 2, false);
                await InsertBookingDetail(voucher, request.RestaurantOp1, booking.BookingId, 3, true);
                await InsertBookingDetail(voucher, request.RestaurantOp2, booking.BookingId, 3, false);
                await InsertBookingDetail(voucher, request.PhotoOp1, booking.BookingId, 4, true);
                await InsertBookingDetail(voucher, request.PhotoOp2, booking.BookingId, 4, false);
                await InsertBookingDetail(voucher, request.EventOp1, booking.BookingId, 6, true);
                await InsertBookingDetail(voucher, request.EventOp2, booking.BookingId, 6, false);

                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();

                _ = SendEmailForSupplier(groom.FullName, bride.FullName, request.ClothesOp1.ServiceId);
                _ = SendEmailForSupplier(groom.FullName, bride.FullName, request.RestaurantOp1.ServiceId);
                _ = SendEmailForSupplier(groom.FullName, bride.FullName, request.PhotoOp1.ServiceId);
                _ = SendEmailForSupplier(groom.FullName, bride.FullName, request.EventOp1.ServiceId);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception(ex.Message, ex);
            }
        }
        //Read
        public async Task<ComboBookingDTO> GetById(int id)
        {
            try
            {
                ComboBookingDTO comboDTO = new();
                ComboBooking? booking = await _bookingRepository.GetAsyncById(id);
                InforBooking groom = await _inforRepository.GetAsyncById(booking.InforGroomId);
                InforBooking bride = await _inforRepository.GetAsyncById(booking.InforBrideId);

                if (booking.TypeCombo == 1)
                {
                    comboDTO.MakeUpOp1 = await GetBookingDetail<CB_MakeUp>(booking.BookingId, 1, true);
                    comboDTO.MakeUpOp2 = await GetBookingDetail<CB_MakeUp>(booking.BookingId, 1, false);
                    comboDTO.InvitationOp1 = await GetBookingDetail<CB_Invitation>(booking.BookingId, 5, true);
                    comboDTO.InvitationOp2 = await GetBookingDetail<CB_Invitation>(booking.BookingId, 5, false);
                }
                comboDTO.ClothesOp1 = await GetBookingDetail<CB_Clothes>(booking.BookingId, 2, true);
                comboDTO.ClothesOp2 = await GetBookingDetail<CB_Clothes>(booking.BookingId, 2, false);
                comboDTO.RestaurantOp1 = await GetBookingDetail<CB_Restaurant>(booking.BookingId, 3, true);
                comboDTO.RestaurantOp2 = await GetBookingDetail<CB_Restaurant>(booking.BookingId, 3, false);
                comboDTO.PhotoOp1 = await GetBookingDetail<CB_Photography>(booking.BookingId, 4, true);
                comboDTO.PhotoOp2 = await GetBookingDetail<CB_Photography>(booking.BookingId, 4, false);
                comboDTO.EventOp1 = await GetBookingDetail<CB_Event>(booking.BookingId, 6, true);
                comboDTO.EventOp2 = await GetBookingDetail<CB_Event>(booking.BookingId, 6, false);

                comboDTO.ExpectedWeddingDate = booking.ExpectedWeddingDate;
                comboDTO.Groom = await ConvetInforBooking(groom);
                comboDTO.Bride = await ConvetInforBooking(bride);
                comboDTO.BookingId = booking.BookingId;
                comboDTO.TypeCombo = booking.TypeCombo;

                return comboDTO;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<RequestPayment>> GetRequestPayment(int userId)
        {
            try
            {
                List<RequestPayment> requests = new();
                List<ComboBooking> combos = await _bookingRepository.GetByUserId(userId);
                List<ComboBooking> filteredCombos = combos.Where(c => c.MinRequiredDeposit > 0).ToList();
                int i = 0;
                foreach (ComboBooking request in filteredCombos)
                {
                    i++;
                    RequestPayment payment = new()
                    {
                        RequestPaymentid = i,
                        Deadline = request.DepositDate ?? DateTime.Now,
                        RequiredAmount = request.MinRequiredDeposit ?? 0,
                        Content = request.Budget == 0
                            ? $"20% deposit payment for combo booking. Automatic payment on {request.DepositDate}"
                            : (request.Budget * 100 / request.TotalPrice <= 20
                                ? $"You need to pay 80% of the total bill to prepare for the upcoming wedding.  Automatic payment on {request.DepositDate}"
                                : $"You need to complete 100% of the total bill for wedding services.  Automatic payment on {request.DepositDate}")
                    };

                    decimal? a = request.Budget * 100 / request.TotalPrice;

                    requests.Add(payment);
                }
                return requests;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        //Update
        public async Task UpdateBooking(int id, ComboBookingDTO request)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                ComboBooking booking = await _bookingRepository.GetAsyncById(id);

                if (booking.BookingStatus != 1)
                    throw new Exception("This booking cannot be edited.");

                InforBooking groom = new()
                {
                    InforId = booking.InforGroomId,
                    FullName = request.Groom.FullName,
                    PhoneNumber = request.Groom.PhoneNumber,
                    City = request.Groom.City,
                    District = request.Groom.District,
                    Ward = request.Groom.Ward,
                    Address = request.Groom.Address,
                    DateOfBirth = request.Groom.DateOfBirth
                };
                InforBooking bride = new()
                {
                    InforId = booking.InforBrideId,
                    FullName = request.Bride.FullName,
                    PhoneNumber = request.Bride.PhoneNumber,
                    City = request.Bride.City,
                    District = request.Bride.District,
                    Ward = request.Bride.Ward,
                    Address = request.Bride.Address,
                    DateOfBirth = request.Bride.DateOfBirth
                };

                await _inforRepository.UpdateAsync(groom.InforId, groom);
                await _inforRepository.UpdateAsync(bride.InforId, bride);

                booking.ExpectedWeddingDate = request.ExpectedWeddingDate;
                await _bookingRepository.UpdateAsync(id, booking);

                if (booking.TypeCombo == 1)
                {
                    await UpdateBookingDetail<CB_MakeUp>(id, 1, true, request.MakeUpOp1);
                    await UpdateBookingDetail<CB_MakeUp>(id, 1, false, request.MakeUpOp2);
                    await UpdateBookingDetail<CB_Invitation>(id, 5, true, request.InvitationOp1);
                    await UpdateBookingDetail<CB_Invitation>(id, 5, false, request.InvitationOp2);
                }
                await UpdateBookingDetail<CB_Clothes>(id, 2, true, request.ClothesOp1);
                await UpdateBookingDetail<CB_Clothes>(id, 2, false, request.ClothesOp2);
                await UpdateBookingDetail<CB_Restaurant>(id, 3, true, request.RestaurantOp1);
                await UpdateBookingDetail<CB_Restaurant>(id, 3, false, request.RestaurantOp2);
                await UpdateBookingDetail<CB_Photography>(id, 4, true, request.PhotoOp1);
                await UpdateBookingDetail<CB_Photography>(id, 4, false, request.PhotoOp2);
                await UpdateBookingDetail<CB_Event>(id, 6, true, request.EventOp1);
                await UpdateBookingDetail<CB_Event>(id, 6, false, request.EventOp2);

                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task SupplierUpdateBooking(int id, int type, SupplierUpdate request)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                BookingServiceDetail detailUpdate = await _detailBookingRepository.GetAsyncById(id);

                ComboBooking booking = await _bookingRepository.GetByDetailId(id);

                VoucherAdmin? voucher = await _voucherRepository.GetAsyncById(booking.VoucherId ?? 0);
                switch (type)
                {
                    case 1:
                        detailUpdate.BasicPrice = request.BasicPrice;
                        detailUpdate.TotalPrice = await UseVoucher(detailUpdate.BasicPrice, voucher);
                        detailUpdate.PackageId = request.PackageId;
                        detailUpdate.Note = request.Note;
                        break;
                    case 2:
                        detailUpdate.BasicPrice = request.BasicPrice;
                        detailUpdate.TotalPrice = await UseVoucher(detailUpdate.BasicPrice, voucher);
                        detailUpdate.Appointment = request.Appointment;
                        detailUpdate.Note = request.Note;
                        break;
                    case 3:
                        detailUpdate.BasicPrice = request.BasicPrice;
                        detailUpdate.TotalPrice = await UseVoucher(detailUpdate.BasicPrice, voucher);
                        detailUpdate.PackageId = request.PackageId;
                        detailUpdate.NumberOfUses = request.NumberOfUses;
                        detailUpdate.Note = request.Note;
                        break;
                    case 4:
                        detailUpdate.BasicPrice = request.BasicPrice;
                        detailUpdate.TotalPrice = await UseVoucher(detailUpdate.BasicPrice, voucher);
                        detailUpdate.PreAppointment = request.PreAppointment;
                        detailUpdate.PrePackageId = request.PrePackageId;
                        detailUpdate.PackageId = request.PackageId;
                        detailUpdate.Note = request.Note;
                        break;
                    case 5:
                        detailUpdate.BasicPrice = request.BasicPrice;
                        detailUpdate.TotalPrice = await UseVoucher(detailUpdate.BasicPrice, voucher);
                        detailUpdate.PackageId = request.PackageId;
                        detailUpdate.Note = request.Note;
                        break;
                    case 6:
                        detailUpdate.BasicPrice = request.BasicPrice;
                        detailUpdate.TotalPrice = await UseVoucher(detailUpdate.BasicPrice, voucher);
                        detailUpdate.PackageId = request.PackageId;
                        detailUpdate.Note = request.Note;
                        break;
                    default:
                        throw new Exception("Service Type incorrect");
                }
                await _detailBookingRepository.UpdateAsync(id, detailUpdate);

                User user = await _detailBookingRepository.GetUserByDetailBooking(id);

                string subject = "Booking Update Notification";
                string content = $"Dear {user.FullName},<br><br>" +
                    $"Your combo booking has been updated by the store {detailUpdate.Service.ServiceName}. Please log in to our system to track the changes and view further details http://localhost:3000/.<br><br>" +
                    "If you have any questions or need further assistance, feel free to contact our support team.<br><br>" +
                    "Best regards,<br>" +
                    "Wedding Wonder Team";

                Message message = new(
                    new List<MailboxAddress> { new(user.FullName, user.Email) },
                    subject,
                    content
                );

                _emailService.SendEmail(message);

                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task ReselectRejected(int id, ReselectRejected request, int typeId)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                ComboBooking booking = await _bookingRepository.GetAsyncById(id);

                if (booking.BookingStatus != 1)
                    throw new Exception("This booking cannot be edited.");

                VoucherAdmin? voucher = await _voucherRepository.GetAsyncById(booking.VoucherId ?? 0);

                switch (typeId)
                {
                    case 1:
                        await InsertBookingDetail(voucher, request.MakeUpOp1, booking.BookingId, 1, true);
                        await InsertBookingDetail(voucher, request.MakeUpOp2, booking.BookingId, 1, false);
                        break;
                    case 2:
                        await InsertBookingDetail(voucher, request.ClothesOp1, booking.BookingId, 2, true);
                        await InsertBookingDetail(voucher, request.ClothesOp2, booking.BookingId, 2, false);
                        break;
                    case 3:
                        await InsertBookingDetail(voucher, request.RestaurantOp1, booking.BookingId, 3, true);
                        await InsertBookingDetail(voucher, request.RestaurantOp2, booking.BookingId, 3, false);
                        break;
                    case 4:
                        await InsertBookingDetail(voucher, request.PhotoOp1, booking.BookingId, 4, true);
                        await InsertBookingDetail(voucher, request.PhotoOp2, booking.BookingId, 4, false);
                        break;
                    case 5:
                        await InsertBookingDetail(voucher, request.InvitationOp1, booking.BookingId, 5, true);
                        await InsertBookingDetail(voucher, request.InvitationOp2, booking.BookingId, 5, false);
                        break;
                    case 6:
                        await InsertBookingDetail(voucher, request.EventOp1, booking.BookingId, 6, true);
                        await InsertBookingDetail(voucher, request.EventOp2, booking.BookingId, 6, false);
                        break;
                    default: throw new Exception("Service Type incorrect");

                }

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

                ComboBooking combo = await _bookingRepository.GetByDetailId(id);
                int initial = combo.BookingStatus;

                await _detailBookingRepository.AcceptServiceDetail(id);
                await _unitOfWork.CommitAsync();

                if (combo.BookingStatus == 2 && initial == 1)
                {
                    await CalculatePrice(combo);

                    User user = await _bookingRepository.GetUserByComboId(combo.BookingId);

                    EmailNotificationEvent emailEvent = new()
                    {
                        Id = Guid.NewGuid(),
                        ReceiverEmail = user.Email,
                        ReceiverName = user.FullName,
                        Subject = "Combo Acceptance Notification",
                        Body = $"Dear {user.FullName},<br><br>" +
                      $"We are pleased to inform you that all services in your selected combo have been accepted and are now confirmed.<br><br>" +
                      "To track the status of your services and view further details, please log in to your account on our system.<br><br>" +
                      "If you have any questions or need further assistance, feel free to contact our support team.<br><br>" +
                      "Best regards,<br>" +
                      "Wedding Wonder Team",
                        TimeStamp = DateTimeOffset.Now
                    };

                    //await _publishEndpoint.Publish(emailEvent);
                }

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

                await _detailBookingRepository.RejectServiceDetail(id);
                await _unitOfWork.CommitAsync();

                ComboBooking combo = await _bookingRepository.GetByDetailId(id);
                if (combo.BookingStatus == 3)
                {
                    User user = await _bookingRepository.GetUserByComboId(combo.BookingId);

                    EmailNotificationEvent emailEvent = new()
                    {
                        Id = Guid.NewGuid(),
                        ReceiverEmail = user.Email,
                        ReceiverName = user.FullName,
                        Subject = "Combo Rejection Notification",
                        Body = $"Dear {user.FullName},<br><br>" +
                       $"We regret to inform you that all services in your selected combo have been rejected by the suppliers.<br><br>" +
                       "To track the status of your services and view further details, please log in to your account on our system.<br><br>" +
                       "If you have any questions or need further assistance, feel free to contact our support team.<br><br>" +
                       "Best regards,<br>" +
                       "Wedding Wonder Team",
                        TimeStamp = DateTimeOffset.Now
                    };

                    await _publishEndpoint.Publish(emailEvent);
                }

                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task CancelAllBooking(int id)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                await _bookingRepository.CancelComboBooking(id);

                List<BookingServiceDetail> allDetailBooking = await _detailBookingRepository.GetByBookingId(id);
                foreach (BookingServiceDetail detail in allDetailBooking)
                    await _detailBookingRepository.CancelServiceDetail(detail.DetailId);

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

                await _detailBookingRepository.CancelServiceDetail(id);

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

                await _detailBookingRepository.UsingServiceDetail(id);

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

                ComboBooking combo = await _bookingRepository.GetByDetailId(id);

                await _detailBookingRepository.FinishServiceDetail(id);
                await _unitOfWork.CommitAsync();

                if (combo.BookingStatus == 5)
                {
                    User user = await _bookingRepository.GetUserByComboId(combo.BookingId);

                    EmailNotificationEvent emailEvent = new()
                    {
                        Id = Guid.NewGuid(),
                        ReceiverEmail = user.Email,
                        ReceiverName = user.FullName,
                        Subject = "Combo Completion Notification",
                        Body = $"Dear {user.FullName},<br><br>" +
                               "We are delighted to inform you that the wedding package services you have used have been successfully completed.<br><br>" +
                               "Please log in to our system at http://localhost:3000/ to rate and review the services you have used.<br><br>" +
                               "If you have any questions or need further assistance, feel free to contact our support team.<br><br>" +
                               "Best regards,<br>" +
                               "Wedding Wonder Team",
                        TimeStamp = DateTimeOffset.Now
                    };

                    await _publishEndpoint.Publish(emailEvent);
                }

                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task ConfirmFinishBooking(int id)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                await _detailBookingRepository.ConfirmFinishServiceDetail(id);

                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception(ex.Message, ex);
            }
        }
        //Delete
        //Others
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
        public async Task<bool> CheckCustomerAndDetailBooking(int customerId, int detailId)
        {
            try
            {
                bool status = await _bookingRepository.CheckCustomerAndDetailBooking(detailId, customerId);
                return status;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<bool> CheckSupplierAndDetailBooking(int supplierId, int detailId)
        {
            try
            {
                bool status = await _bookingRepository.CheckSupplierAndBooking(detailId, supplierId);
                return status;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        private async Task InsertBookingDetail(VoucherAdmin voucher, object detail, int bookingId, int typeId, bool priority)
        {
            try
            {
                BookingServiceDetail serviceDetail = new()
                {
                    BookingId = bookingId,
                    ServiceTypeId = typeId,
                    Status = 1,
                    Priority = priority
                };
                switch (typeId)
                {
                    case 1:
                        CB_MakeUp? makeUpDetail = detail as CB_MakeUp;
                        if (makeUpDetail != null)
                        {
                            serviceDetail.BasicPrice = makeUpDetail.Price ?? 0;
                            serviceDetail.ServiceId = makeUpDetail.ServiceId;
                            serviceDetail.PackageId = makeUpDetail.PackageId;
                            serviceDetail.ServiceMode = makeUpDetail.ServiceMode;
                            serviceDetail.Note = makeUpDetail.Note;
                        }
                        else
                        {
                            throw new InvalidCastException("Invalid detail type for MakeUp service.");
                        }
                        break;
                    case 2:
                        CB_Clothes? clothesDetail = detail as CB_Clothes;
                        if (clothesDetail != null)
                        {
                            serviceDetail.ServiceId = clothesDetail.ServiceId;
                            serviceDetail.Appointment = clothesDetail.FittingDay;
                            serviceDetail.Note = clothesDetail.Note;
                            serviceDetail.BasicPrice = 0;
                        }
                        else
                        {
                            throw new InvalidCastException("Invalid detail type for Clothes service.");
                        }
                        break;
                    case 3:
                        CB_Restaurant? restaurantDetail = detail as CB_Restaurant;
                        if (restaurantDetail != null)
                        {
                            serviceDetail.BasicPrice = restaurantDetail.Price ?? 0;
                            serviceDetail.ServiceId = restaurantDetail.ServiceId;
                            serviceDetail.PackageId = restaurantDetail.MenuId;
                            serviceDetail.NumberOfUses = restaurantDetail.NumberOfUses;
                            serviceDetail.Note = restaurantDetail.Note;
                        }
                        else
                        {
                            throw new InvalidCastException("Invalid detail type for Restaurant service.");
                        }
                        break;
                    case 4:
                        CB_Photography? photographyDetail = detail as CB_Photography;
                        if (photographyDetail != null)
                        {
                            serviceDetail.PreAppointment = photographyDetail.PreAppointment;
                            serviceDetail.ServiceId = photographyDetail.ServiceId;
                            serviceDetail.PrePackageId = photographyDetail.PreWeddingPackageId;
                            serviceDetail.PackageId = photographyDetail.WeddingPackageId;
                            serviceDetail.BasicPrice = photographyDetail.Price ?? 0;
                            serviceDetail.Note = photographyDetail.Note;
                        }
                        else
                        {
                            throw new InvalidCastException("Invalid detail type for Photography service.");
                        }
                        break;
                    case 5:
                        CB_Invitation? invitationDetail = detail as CB_Invitation;
                        if (invitationDetail != null)
                        {
                            serviceDetail.ServiceId = invitationDetail.ServiceId;
                            serviceDetail.PackageId = invitationDetail.InvitationId;
                            serviceDetail.Note = invitationDetail.Note;
                            serviceDetail.ServiceMode = invitationDetail.ServiceMode;
                            serviceDetail.BasicPrice = invitationDetail.Price ?? 0;
                            serviceDetail.NumberOfUses = invitationDetail.NumberOfUses;
                        }
                        else
                        {
                            throw new InvalidCastException("Invalid detail type for Invitation service.");
                        }
                        break;
                    case 6:
                        CB_Event? eventDetail = detail as CB_Event;
                        if (eventDetail != null)
                        {
                            serviceDetail.BasicPrice = eventDetail.Price ?? 0;
                            serviceDetail.ServiceId = eventDetail.ServiceId;
                            serviceDetail.PackageId = eventDetail.ConceptId;
                            serviceDetail.Note = eventDetail.Note;
                        }
                        else
                        {
                            throw new InvalidCastException("Invalid detail type for Event service.");
                        }
                        break;
                    default:
                        throw new Exception("Service Type incorrect");
                }
                serviceDetail.TotalPrice = await UseVoucher(serviceDetail.BasicPrice, voucher);

                if (priority) await _detailBookingRepository.CreateAsync(serviceDetail);
                else await _detailBookingRepository.CreateServiceDetailNotPriority(serviceDetail);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        private async Task<T?> GetBookingDetail<T>(int bookingId, int typeId, bool priority) where T : class
        {
            try
            {
                BookingServiceDetail? detail = null;
                BookingServiceDetail? detailOp1 = await _detailBookingRepository.GetByBookingIdAndServiceTypeOp1(bookingId, typeId);
                BookingServiceDetail? detailOp2 = await _detailBookingRepository.GetByBookingIdAndServiceTypeOp2(bookingId, typeId);

                if (priority)
                {
                    if (detailOp1 == null) detail = detailOp2;
                    else detail = detailOp1;
                }
                else
                {
                    if (detailOp1 == null) return null;
                    else
                    {
                        if (detailOp1.Status != 1) return null;
                        detail = detailOp2;
                    }
                }

                switch (typeId)
                {
                    case 1:
                        if (detail != null)
                        {
                            CB_MakeUp makeUpDetail = new()
                            {
                                ServiceId = detail.ServiceId,
                                PackageId = detail.PackageId ?? 0,
                                Note = detail.Note,
                                Price = detail.BasicPrice,
                                Status = detail.Status
                            };
                            return makeUpDetail as T;
                        }
                        else
                        {
                            return null;
                        }
                    case 2:
                        if (detail != null)
                        {
                            CB_Clothes clothesDetail = new()
                            {
                                ServiceId = detail.ServiceId,
                                FittingDay = detail.Appointment ?? DateTime.Now,
                                Note = detail.Note,
                                Price = detail.BasicPrice,
                                Status = detail.Status
                            };
                            return clothesDetail as T;
                        }
                        else
                        {
                            return null;
                        }
                    case 3:
                        if (detail != null)
                        {
                            int cuisineTypeId = (await _cateringRepository.GetCateringByMenuId(detail.PackageId ?? 0)).CateringId;
                            CB_Restaurant restaurantDetail = new()
                            {
                                ServiceId = detail.ServiceId,
                                CuisineTypeId = cuisineTypeId,
                                MenuId = detail.PackageId ?? 0,
                                NumberOfUses = detail.NumberOfUses,
                                Note = detail.Note,
                                Price = detail.BasicPrice,
                                Status = detail.Status
                            };
                            return restaurantDetail as T;
                        }
                        else
                        {
                            return null;
                        }
                    case 4:
                        if (detail != null)
                        {
                            CB_Photography photographyDetail = new()
                            {
                                PreAppointment = detail.PreAppointment ?? DateTime.Now,
                                ServiceId = detail.ServiceId,
                                PreWeddingPackageId = detail.PrePackageId ?? 0,
                                WeddingPackageId = detail.PackageId ?? 0,
                                Note = detail.Note,
                                Price = detail.BasicPrice,
                                Status = detail.Status
                            };
                            return photographyDetail as T;
                        }
                        else
                        {
                            return null;
                        }
                    case 5:
                        if (detail != null)
                        {
                            CB_Invitation invitationDetail = new()
                            {
                                ServiceId = detail.ServiceId,
                                InvitationId = detail.PackageId ?? 0,
                                Note = detail.Note,
                                Price = detail.BasicPrice,
                                Status = detail.Status
                            };
                            return invitationDetail as T;
                        }
                        else
                        {
                            return null;
                        }
                    case 6:
                        if (detail != null)
                        {
                            int packageId = (await _eventRepository.GetEventByConceptId(detail.PackageId ?? 0)).PackageId;
                            CB_Event eventDetail = new()
                            {
                                ServiceId = detail.ServiceId,
                                PackageId = packageId,
                                ConceptId = detail.PackageId ?? 0,
                                Note = detail.Note,
                                Price = detail.BasicPrice,
                                Status = detail.Status
                            };
                            return eventDetail as T;
                        }
                        else
                        {
                            return null;
                        }
                    default:
                        throw new Exception("Service Type incorrect");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        private async Task UpdateBookingDetail<T>(int bookingId, int typeId, bool priority, T detail) where T : class
        {
            try
            {
                BookingServiceDetail? detailUpdate = priority
                        ? await _detailBookingRepository.GetByBookingIdAndServiceTypeOp1(bookingId, typeId)
                        : await _detailBookingRepository.GetByBookingIdAndServiceTypeOp2(bookingId, typeId);
                switch (typeId)
                {
                    case 1:
                        CB_MakeUp? makeUpDetail = detail as CB_MakeUp;
                        if (makeUpDetail != null)
                        {
                            detailUpdate.ServiceId = makeUpDetail.ServiceId;
                            detailUpdate.PackageId = makeUpDetail.PackageId;
                            detailUpdate.Note = makeUpDetail.Note;
                        }
                        else
                        {
                            throw new InvalidCastException("Invalid detail type for MakeUp service.");
                        }
                        break;
                    case 2:
                        CB_Clothes? clothesDetail = detail as CB_Clothes;
                        if (clothesDetail != null)
                        {
                            detailUpdate.ServiceId = clothesDetail.ServiceId;
                            detailUpdate.Appointment = clothesDetail.FittingDay;
                            detailUpdate.Note = clothesDetail.Note;
                        }
                        else
                        {
                            throw new InvalidCastException("Invalid detail type for Clothes service.");
                        }
                        break;
                    case 3:
                        CB_Restaurant? restaurantDetail = detail as CB_Restaurant;
                        if (restaurantDetail != null)
                        {
                            detailUpdate.ServiceId = restaurantDetail.ServiceId;
                            detailUpdate.PackageId = restaurantDetail.MenuId;
                            detailUpdate.NumberOfUses = restaurantDetail.NumberOfUses;
                            detailUpdate.Note = restaurantDetail.Note;
                        }
                        else
                        {
                            throw new InvalidCastException("Invalid detail type for Restaurant service.");
                        }
                        break;
                    case 4:
                        CB_Photography? photographyDetail = detail as CB_Photography;
                        if (photographyDetail != null)
                        {
                            detailUpdate.PreAppointment = photographyDetail.PreAppointment;
                            detailUpdate.ServiceId = photographyDetail.ServiceId;
                            detailUpdate.PrePackageId = photographyDetail.PreWeddingPackageId;
                            detailUpdate.PackageId = photographyDetail.WeddingPackageId;
                            detailUpdate.Note = photographyDetail.Note;
                        }
                        else
                        {
                            throw new InvalidCastException("Invalid detail type for Photography service.");
                        }
                        break;
                    case 5:
                        CB_Invitation? invitationDetail = detail as CB_Invitation;
                        if (invitationDetail != null)
                        {
                            detailUpdate.ServiceId = invitationDetail.ServiceId;
                            detailUpdate.PackageId = invitationDetail.InvitationId;
                            detailUpdate.Note = invitationDetail.Note;
                        }
                        else
                        {
                            throw new InvalidCastException("Invalid detail type for Invitation service.");
                        }
                        break;
                    case 6:
                        CB_Event? eventDetail = detail as CB_Event;
                        if (eventDetail != null)
                        {
                            detailUpdate.ServiceId = eventDetail.ServiceId;
                            detailUpdate.PackageId = eventDetail.ConceptId;
                            detailUpdate.Note = eventDetail.Note;
                        }
                        else
                        {
                            throw new InvalidCastException("Invalid detail type for Event service.");
                        }
                        break;
                    default:
                        throw new Exception("Service Type incorrect");
                }
                await _detailBookingRepository.UpdateAsync(bookingId, detailUpdate);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public static async Task<InforBookingDTO> ConvetInforBooking(InforBooking infor)
        {
            return await Task.FromResult(new InforBookingDTO()
            {
                FullName = infor.FullName,
                PhoneNumber = infor.PhoneNumber,
                City = infor.City,
                District = infor.District,
                Ward = infor.Ward,
                Address = infor.Address,
                DateOfBirth = infor.DateOfBirth,
                InforId = infor.InforId
            });
        }
        private async Task SendEmailForSupplier(string groom, string bride, int serviceID)
        {
            Service service = await _serviceRepository.GetAsyncById(serviceID);

            string subject = "Combo Acceptance Notification";
            string content = $"Dear {service.Supplier.FullName},<br><br>" +
                $"Your store {service.ServiceName} has an booking for the wedding combo services of the bride {bride} and groom {groom} in our system.<br><br>" +
                "To track the status of your services and view further details, please log in to your account on our system.<br><br>" +
                "If you have any questions or need further assistance, feel free to contact our support team.<br><br>" +
                "Best regards,<br>" +
                "Wedding Wonder Team";

            EmailNotificationEvent emailEvent = new()
            {
                Id = Guid.NewGuid(),
                ReceiverEmail = service.Supplier.Email,
                ReceiverName = service.Supplier.FullName,
                Subject = subject,
                Body = content,
                TimeStamp = DateTimeOffset.Now
            };

            await _publishEndpoint.Publish(emailEvent);
        }
        private async Task<decimal> UseVoucher(decimal basicPrice, VoucherAdmin voucher)
        {
            voucher ??= new VoucherAdmin
            {
                TypeOfDiscount = 2,
                Amount = 0
            };
            decimal totalPrice = basicPrice;
            if (voucher.TypeOfDiscount == 1)
            {
                decimal percent = voucher.Percent ?? 0;
                totalPrice = basicPrice * (100 - percent) / 100;
            }
            else if (voucher.TypeOfDiscount == 2)
            {
                decimal amount = voucher.Amount ?? 0;
                totalPrice = basicPrice - amount;
            }
            return totalPrice < 0 ? 0 : totalPrice;
        }
        private async Task CalculatePrice(ComboBooking combo)
        {
            List<BookingServiceDetail> details = (await _detailBookingRepository
                .GetByBookingId(combo.BookingId))
                .Where(d => d.Status == 2)
                .ToList();

            combo.BasicPrice = details.Sum(d => d.BasicPrice);
            combo.TotalPrice = details.Sum(d => d.TotalPrice);
            combo.Budget = 0;
            combo.DepositDate = DateTime.Now.AddDays(3).Date.AddSeconds(-1);
            combo.MinRequiredDeposit = combo.TotalPrice * 20 / 100;
        }
    }
}

using BusinessObject.Contracts.IntergrationEvents;
using BusinessObject.Models;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Repository.IRepositories;
using Services.Email;

namespace Services
{
    public class BGService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        private readonly IEmailService _emailService;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IComboBookingRepository _bookingRepository;
        private readonly IBookingServiceDetailRepository _detailRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITransactionRepository _transactionRepository;

        public BGService(
            IConfiguration configuration,
            IUnitOfWork unitOfWork,
            IComboBookingRepository bookingRepository,
            IBookingServiceDetailRepository detailRepository,
            IUserRepository userRepository,
            IPublishEndpoint publishEndpoint,
            ITransactionRepository transactionRepository,
            IEmailService emailService)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _bookingRepository = bookingRepository;
            _detailRepository = detailRepository;
            _userRepository = userRepository;
            _publishEndpoint = publishEndpoint;
            _transactionRepository = transactionRepository;
            _emailService = emailService;
        }

        public async Task ChangeStatusOp1Op2()
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                List<BookingServiceDetail> details = await _detailRepository.GetsAsync();
                if (details == null) return;

                Op1Overdues(details);
                Op2Overdues(details);

                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task ChangeStatusConfirmFinish()
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                List<BookingServiceDetail> details = await _detailRepository.GetsAsync();
                if (details == null) return;

                details.Where(b => b.Status == 5
                        && (DateTime.Now - b.Booking.ExpectedWeddingDate).TotalHours >= 48)
                    .ToList().ForEach(b => b.Status = 6);
                await _unitOfWork.CommitAsync();

                List<ComboBooking> bookings = await _bookingRepository.GetsAsync();
                bookings.Where(b => b.BookingStatus == 5
                        && (DateTime.Now - b.ExpectedWeddingDate).TotalHours >= 48)
                    .ToList().ForEach(b => b.BookingStatus = 6);

                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task ChangeStatusRejectAll()
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                List<BookingServiceDetail> details = await _detailRepository.GetsAsync();
                if (details == null) return;

                List<List<BookingServiceDetail>> comboCanceled = details
                    .GroupBy(detail => new { detail.BookingId, detail.Priority })
                    .Where(group => group.Any(detail =>
                        detail.Priority == false &&
                        (detail.Status == 0 || detail.Status == 3)))
                    .Select(group => group.ToList())
                    .ToList();

                foreach (List<BookingServiceDetail>? bookingDetails in comboCanceled)
                {
                    if (bookingDetails.Count > 0 && bookingDetails[0].Booking != null)
                    {
                        bookingDetails[0].Booking.BookingStatus = 3;
                    }
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
        public async Task PaymentNotice()
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                List<ComboBooking> combos = await _bookingRepository.GetsAsync();
                if (combos == null) return;

                List<ComboBooking> filteredCombos = combos.Where(c =>
                ((c.ExpectedWeddingDate - DateTime.Now).TotalDays <= 7
                || (DateTime.Now - c.ExpectedWeddingDate).TotalDays >= 1)
                && c.MinRequiredDeposit == 0
                && (c.BookingStatus == 2 || c.BookingStatus == 4 || c.BookingStatus == 5 || c.BookingStatus == 6)).ToList();

                foreach (ComboBooking combo in filteredCombos)
                {
                    User user = await _userRepository.GetAsyncById(combo.UserId);
                    if (combo.Budget * 100 / combo.TotalPrice <= 20)
                        combo.MinRequiredDeposit = combo.TotalPrice / 100 * 80 - combo.Budget;
                    else combo.MinRequiredDeposit = combo.TotalPrice - combo.Budget;

                    EmailNotificationEvent emailEvent = new()
                    {
                        Id = Guid.NewGuid(),
                        ReceiverEmail = user.Email,
                        ReceiverName = user.FullName,
                        Subject = "Payment Request Notification",
                        Body = $"Dear {user.FullName},<br><br>" +
                            $"You have a combo booking that needs to be paid by the due date {combo.DepositDate} for the amount of {combo.MinRequiredDeposit} <br><br>" +
                            "To track your service status and view more details, please log in to your account on our system.<br><br>" +
                            "If you have any questions or need further assistance, please contact our support team.<br><br>" +
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
        public async Task AutomationPayment()
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                List<ComboBooking> combos = await _bookingRepository.GetsAsync();
                if (combos == null) return;

                List<ComboBooking> comboType1 = combos
                    .Where(c => c.DepositDate.HasValue && (DateTime.Now - c.DepositDate.Value).TotalDays >= 0
                    && c.BookingStatus == 2 && c.Budget == 0).ToList();

                List<ComboBooking> comboType2 = combos
                    .Where(c => c.DepositDate.HasValue && (DateTime.Now - c.DepositDate.Value).TotalDays >= 0
                    && (c.BookingStatus == 2 || c.BookingStatus == 4) && c.Budget / c.TotalPrice * 100 <= 20 && c.Budget != 0).ToList();

                List<ComboBooking> comboType3 = combos
                    .Where(c => c.DepositDate.HasValue && (DateTime.Now - c.DepositDate.Value).TotalDays >= 0
                    && (c.BookingStatus == 2 || c.BookingStatus == 4 || c.BookingStatus == 5 || c.BookingStatus == 6)
                    && c.Budget / c.TotalPrice * 100 > 20).ToList();

                await PaymentType1(comboType1);
                await PaymentType2(comboType2);
                await PaymentType3(comboType3);

                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();

            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception(ex.Message, ex);
            }
        }
        private static void Op1Overdues(List<BookingServiceDetail> details)
        {
            List<List<BookingServiceDetail>> op1Overdues = details
                .GroupBy(detail => new { detail.BookingId, detail.ServiceTypeId })
                .Where(group => group.Any(detail =>
                    detail.Priority == true &&
                    detail.Status == 1 &&
                    (DateTime.Now - detail.Booking.CreatedAt).TotalHours >= 12))
                .Select(group => group.ToList())
                .ToList();

            foreach (List<BookingServiceDetail> booking in op1Overdues)
            {
                // reject op1 khi quá 12h
                BookingServiceDetail? op1Detail = booking
                    .Where(detail => detail.Priority == true &&
                                        detail.Status == 1 &&
                                        (DateTime.Now - detail.Booking.CreatedAt).TotalHours >= 12)
                    .FirstOrDefault();

                op1Detail.Status = 3;
                //await SendEmailReject(op1Detail);

                // swap qua op2 khi op1 quá 24h
                BookingServiceDetail? op2Detail = booking
                    .Where(detail => detail.Priority == false && detail.Status == 7)
                    .FirstOrDefault();

                op2Detail.Status = 1;
                //await SendEmailSwap(op2Detail);
            }
        }
        private static void Op2Overdues(List<BookingServiceDetail> details)
        {
            List<BookingServiceDetail> op2Overdues = details
                .Where(detail =>
                    detail.Priority == false &&
                    detail.Status == 1 &&
                    (DateTime.Now - detail.Booking.CreatedAt).TotalHours >= 24)
                .ToList();

            foreach (BookingServiceDetail op2DetailReject in op2Overdues)
            {
                // reject op2 khi quá hạn

                op2DetailReject.Status = 3;
                //await SendEmailReject(op2DetailReject);
            }
        }
        private async Task SendEmailReject(BookingServiceDetail? detailReject)
        {
            EmailNotificationEvent emailEvent = new()
            {
                Id = Guid.NewGuid(),
                ReceiverEmail = detailReject.Service.Supplier.Email,
                ReceiverName = detailReject.Service.Supplier.FullName,
                Subject = "Automatic Reject Notification",
                Body = $"Dear {detailReject.Service.Supplier.FullName},<br><br>" +
                    $"You have a booking from a customer {detailReject.Booking.User.FullName} but the time limit has passed, we regret to inform you that your booking has been automatically cancelled.<br><br>" +
                    "To track your service status and view more details, please log in to your account on our system.<br><br>" +
                    "If you have any questions or need further assistance, please contact our support team.<br><br>" +
                    "Best regards,<br>" +
                    "Wedding Wonder Team",
                TimeStamp = DateTimeOffset.Now
            };

            await _publishEndpoint.Publish(emailEvent);
        }
        private async Task SendEmailSwap(BookingServiceDetail? detailSwap)
        {
            EmailNotificationEvent emailEvent = new()
            {
                Id = Guid.NewGuid(),
                ReceiverEmail = detailSwap.Service.Supplier.Email,
                ReceiverName = detailSwap.Service.Supplier.FullName,
                Subject = "Automatic Reject Notification",
                Body = $"Dear {detailSwap.Service.Supplier.FullName},<br><br>" +
                    $"You have a booking from customer {detailSwap.Booking.User.FullName} for service {detailSwap.Service.ServiceName} on our system <br><br>" +
                    "To track the status of your service and view more details, please log in to your account on our system.<br><br>" +
                    "If you have any questions or need further assistance, please contact our support team.<br><br>" +
                    "Best regards,<br>" +
                    "Wedding Wonder Team",
                TimeStamp = DateTimeOffset.Now
            };

            await _publishEndpoint.Publish(emailEvent);
        }
        private async Task PaymentType1(List<ComboBooking> combos)
        {

            foreach (ComboBooking combo in combos)
            {
                User user = await _userRepository.GetAsyncById(combo.UserId);
                if (combo.MinRequiredDeposit.HasValue && user.Balance >= (double)combo.MinRequiredDeposit.Value)
                {
                    Transaction transaction = new()
                    {
                        Amount = (double)combo.MinRequiredDeposit.Value,
                        Reason = "Automatic payment for services: Wedding service deposit.",
                        RequestDate = DateTime.Now,
                        TransactionType = "Deposit",
                        UserId = user.UserId
                    };
                    await _transactionRepository.CreateAsync(transaction);

                    user.Balance -= (double)combo.MinRequiredDeposit.Value;
                    combo.Budget += combo.MinRequiredDeposit;
                    combo.MinRequiredDeposit = 0;
                    combo.DepositDate = combo.ExpectedWeddingDate.AddDays(-2).Date.AddSeconds(-1);
                }

                EmailNotificationEvent emailEvent = new()
                {
                    Id = Guid.NewGuid(),
                    ReceiverEmail = user.Email,
                    ReceiverName = user.FullName,
                    Subject = "Payment Request Notification",
                    Body = $"Dear {user.FullName},<br><br>" +
                        $"You have a combo booking that needs to be paid by the due date {combo.DepositDate} for the amount of {combo.MinRequiredDeposit} for Wedding service deposit<br><br>" +
                        "To track your service status and view more details, please log in to your account on our system.<br><br>" +
                        "If you have any questions or need further assistance, please contact our support team.<br><br>" +
                        "Best regards,<br>" +
                        "Wedding Wonder Team",
                    TimeStamp = DateTimeOffset.Now
                };
                //await _publishEndpoint.Publish(emailEvent);
            }
        }
        private async Task PaymentType2(List<ComboBooking> combos)
        {

            foreach (ComboBooking combo in combos)
            {
                User user = await _userRepository.GetAsyncById(combo.UserId);
                if (combo.MinRequiredDeposit.HasValue && user.Balance >= (double)combo.MinRequiredDeposit.Value)
                {
                    Transaction transaction = new()
                    {
                        Amount = (double)combo.MinRequiredDeposit.Value,
                        Reason = "Automatic payment for services: Prepare to do the wedding right away.",
                        RequestDate = DateTime.Now,
                        TransactionType = "Deposit",
                        UserId = user.UserId
                    };
                    await _transactionRepository.CreateAsync(transaction);

                    user.Balance -= (double)combo.MinRequiredDeposit.Value;
                    combo.Budget += combo.MinRequiredDeposit;
                    combo.MinRequiredDeposit = 0;
                    combo.DepositDate = combo.ExpectedWeddingDate.AddDays(3).Date.AddSeconds(-1);
                }

                EmailNotificationEvent emailEvent = new()
                {
                    Id = Guid.NewGuid(),
                    ReceiverEmail = user.Email,
                    ReceiverName = user.FullName,
                    Subject = "Payment Request Notification",
                    Body = $"Dear {user.FullName},<br><br>" +
                        $"You have a combo booking that needs to be paid by the due date {combo.DepositDate} for the amount of {combo.MinRequiredDeposit} for Prepare to do the wedding right away<br><br>" +
                        "To track your service status and view more details, please log in to your account on our system.<br><br>" +
                        "If you have any questions or need further assistance, please contact our support team.<br><br>" +
                        "Best regards,<br>" +
                        "Wedding Wonder Team",
                    TimeStamp = DateTimeOffset.Now
                };
                //await _publishEndpoint.Publish(emailEvent);
            }
        }
        private async Task PaymentType3(List<ComboBooking> combos)
        {

            foreach (ComboBooking combo in combos)
            {
                User user = await _userRepository.GetAsyncById(combo.UserId);
                if (combo.MinRequiredDeposit.HasValue && user.Balance >= (double)combo.MinRequiredDeposit.Value)
                {
                    Transaction transaction = new()
                    {
                        Amount = (double)combo.MinRequiredDeposit.Value,
                        Reason = "Automatic payment for services: Pay 100% of all wedding service costs.",
                        RequestDate = DateTime.Now,
                        TransactionType = "Deposit",
                        UserId = user.UserId
                    };
                    await _transactionRepository.CreateAsync(transaction);

                    user.Balance -= (double)combo.MinRequiredDeposit.Value;
                    combo.Budget += combo.MinRequiredDeposit;
                    combo.MinRequiredDeposit = 0;
                }

                EmailNotificationEvent emailEvent = new()
                {
                    Id = Guid.NewGuid(),
                    ReceiverEmail = user.Email,
                    ReceiverName = user.FullName,
                    Subject = "Payment Request Notification",
                    Body = $"Dear {user.FullName},<br><br>" +
                        $"You have a combo booking that needs to be paid by the due date {combo.DepositDate} for the amount of {combo.MinRequiredDeposit} for Pay 100% of all wedding service costs<br><br>" +
                        "To track your service status and view more details, please log in to your account on our system.<br><br>" +
                        "If you have any questions or need further assistance, please contact our support team.<br><br>" +
                        "Best regards,<br>" +
                        "Wedding Wonder Team",
                    TimeStamp = DateTimeOffset.Now
                };
                //await _publishEndpoint.Publish(emailEvent);
            }
        }
    }
}

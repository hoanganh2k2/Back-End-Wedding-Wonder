using BusinessObject.Contracts.IntergrationEvents;
using BusinessObject.DTOs;
using BusinessObject.Models;
using MassTransit;
using Repository.IRepositories;
using Services.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class RequestUpgradeSupplierService
    {
        private readonly IRequestUpgradeSupplierRepository _requestRepository;
        private readonly IRequestImageRepository _imageRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IUnitOfWork _unitOfWork;

        public RequestUpgradeSupplierService(
            IRequestUpgradeSupplierRepository requestRepository,
            IRequestImageRepository imageRepository,
            IUnitOfWork unitOfWork,
            IUserRepository userRepository,
            IEmailService emailService,
            IPublishEndpoint publishEndpoint)
        {
            _requestRepository = requestRepository;
            _imageRepository = imageRepository;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _emailService = emailService;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<List<RequestUpgradeSupplierDTO>> GetPendingRequestsAsync()
        {
            try
            {
                var pendingRequests = await _requestRepository.GetRequestsByStatusAsync("Pending");

                foreach (var request in pendingRequests)
                {
                    request.RequestImages = await _imageRepository.GetImagesByRequestIdAsync(request.RequestId);
                }

                return pendingRequests;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách yêu cầu chờ xử lý: {ex.Message}", ex);
            }
        }

        public async Task<bool> CreateRequestAsync(RequestUpgradeSupplierDTO requestDto, List<RequestImageDTO> imageDtos)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var request = new RequestUpgradeSupplier
                {
                    UserId = requestDto.UserId,
                    RequestContent = requestDto.RequestContent,
                    BusinessType = requestDto.BusinessType,
                    Status = "Pending",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    IdNumber = requestDto.IdNumber,
                    FullName = requestDto.FullName
                };

                var isRequestAdded = await _requestRepository.AddRequestAsync(request);
                await _unitOfWork.CommitAsync();

                if (!isRequestAdded)
                {
                    throw new Exception("Failed to create request.");
                }

                var images = imageDtos.Select(dto => new RequestImage
                {
                    RequestId = request.RequestId,
                    ImageText = dto.ImageText,
                    ImageType = dto.ImageType
                }).ToList();

                var areImagesAdded = await _imageRepository.AddImagesAsync(images);
                if (!areImagesAdded)
                {
                    throw new Exception("Failed to add images.");
                }

                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
                return true;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception($"Error creating request: {ex.Message}", ex);
            }
        }

        public async Task<bool> AcceptRequestAsync(int requestId)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var requestDto = await _requestRepository.GetRequestByIdAsync(requestId);
                if (requestDto == null)
                    throw new Exception("Không tìm thấy yêu cầu.");

                int? userId = requestDto.UserId;
                if (userId == null)
                    throw new Exception("Thông tin người dùng không hợp lệ.");

                var request = new RequestUpgradeSupplier
                {
                    RequestId = requestDto.RequestId,
                    UserId = requestDto.UserId,
                    RequestContent = requestDto.RequestContent,
                    BusinessType = requestDto.BusinessType,
                    Status = "Approved",
                    RejectReason = null,
                    CreatedAt = requestDto.CreatedAt,
                    UpdatedAt = DateTime.UtcNow,
                    IdNumber = requestDto.IdNumber,
                    FullName = requestDto.FullName
                };

                var isUpdated = await _requestRepository.UpdateRequestAsync(request);
                if (!isUpdated)
                    throw new Exception("Cập nhật trạng thái yêu cầu thất bại.");

                var user = await _userRepository.GetAsyncById(userId.Value);
                if (user == null)
                    throw new Exception("Không tìm thấy thông tin người dùng.");

                user.RoleId = 3;
                user.IsUpgradeConfirmed = 0;

                await _userRepository.UpdateAsync(userId.Value, user);
                await _unitOfWork.CommitAsync();

                string subject = "Yêu cầu nâng cấp tài khoản được chấp nhận";
                string content = $"Kính gửi {user.FullName},<br><br>" +
                    $"Yêu cầu nâng cấp tài khoản của bạn đã được chấp nhận.<br>" +
                    "Bạn hiện đã là nhà cung cấp chính thức trên hệ thống của chúng tôi.<br><br>" +
                    "Cảm ơn bạn đã lựa chọn sử dụng dịch vụ của chúng tôi.<br><br>" +
                    "Trân trọng,<br>Đội ngũ Wedding Wonder";

                var emailEvent = new EmailNotificationEvent
                {
                    Id = Guid.NewGuid(),
                    ReceiverEmail = user.Email,
                    ReceiverName = user.FullName,
                    Subject = subject,
                    Body = content,
                    TimeStamp = DateTimeOffset.Now
                };

                await _publishEndpoint.Publish(emailEvent);

                var notificationEvent = new MessageNotificationEvent
                {
                    Id = Guid.NewGuid(),
                    TimeStamp = DateTimeOffset.Now,
                    SenderId = 1,
                    ReceiverId = userId.Value,
                    MessageContent = $"Your account upgrade request has been accepted.",
                    NotificationType = "RequestAccepted",
                    Name = "Account upgrade successful",
                    Description = "Your account has been upgraded to a provider."
                };

                await _publishEndpoint.Publish(notificationEvent);
                await _unitOfWork.CommitTransactionAsync();
                return true;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception($"Lỗi khi chấp nhận yêu cầu: {ex.Message}", ex);
            }
        }

        public async Task<bool> RejectRequestAsync(int requestId, string rejectReason)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var requestDto = await _requestRepository.GetRequestByIdAsync(requestId);
                if (requestDto == null)
                    throw new Exception("Request not found.");

                int? userId = requestDto.UserId;
                if (userId == null)
                    throw new Exception("Thông tin người dùng không hợp lệ.");

                var request = new RequestUpgradeSupplier
                {
                    RequestId = requestDto.RequestId,
                    UserId = requestDto.UserId,
                    RequestContent = requestDto.RequestContent,
                    BusinessType = requestDto.BusinessType,
                    Status = "Rejected",
                    RejectReason = rejectReason,
                    CreatedAt = requestDto.CreatedAt,
                    UpdatedAt = DateTime.UtcNow,
                    IdNumber = requestDto.IdNumber,
                    FullName = requestDto.FullName
                };

                var isUpdated = await _requestRepository.UpdateRequestAsync(request);
                if (!isUpdated)
                    throw new Exception("Failed to update request status.");

                var user = await _userRepository.GetAsyncById(userId.Value);
                if (user == null)
                    throw new Exception("User not found.");

                await _unitOfWork.CommitAsync();

                string subject = "Yêu cầu nâng cấp tài khoản bị từ chối";
                string content = $"Kính gửi {user.FullName},<br><br>" +
                    $"Yêu cầu nâng cấp tài khoản của bạn đã bị từ chối.<br>" +
                    $"Lý do từ chối: {rejectReason}<br><br>" +
                    "Vui lòng liên hệ với đội ngũ hỗ trợ để biết thêm chi tiết.<br><br>" +
                    "Trân trọng,<br>Wedding Wonder Team";

                var emailEvent = new EmailNotificationEvent
                {
                    Id = Guid.NewGuid(),
                    ReceiverEmail = user.Email,
                    ReceiverName = user.FullName,
                    Subject = subject,
                    Body = content,
                    TimeStamp = DateTimeOffset.Now
                };

                await _publishEndpoint.Publish(emailEvent);

                await _unitOfWork.CommitTransactionAsync();
                return true;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception($"Error rejecting request: {ex.Message}", ex);
            }
        }
        public async Task<bool> ConfirmUpgradeAsync(int userId)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                var user = await _userRepository.GetAsyncById(userId);
                if (user == null)
                    throw new Exception("Không tìm thấy người dùng.");

                if (user.RoleId != 3 || user.IsUpgradeConfirmed == 1)
                    throw new Exception("Người dùng không cần xác nhận hoặc đã xác nhận.");

                user.IsUpgradeConfirmed = 1;
                await _userRepository.UpdateAsync(userId, user);
                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xác nhận nâng cấp: {ex.Message}", ex);
            }
        }

        public async Task<List<string?>> GetApprovedBusinessTypesAsync(int userId)
        {
            var approvedRequests = await _requestRepository.GetRequestsByStatusAsync("Approved");

            return approvedRequests
                .Where(req => req.UserId == userId)
                .Select(req => req.BusinessType)
                .Distinct()
                .ToList();
        }
    }
}

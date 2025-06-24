using BusinessObject.DTOs;
using BusinessObject.Models;
using Repository.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class NotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public NotificationService(INotificationRepository notificationRepository, IUnitOfWork unitOfWork)
        {
            _notificationRepository = notificationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<NotificationDTO>> GetUserNotifications(int userId)
        {
            var notifications = await _notificationRepository.GetUnreadNotificationsAsync(userId);
            return notifications.Select(n => new NotificationDTO
            {
                Id = n.Id,
                ReceiverId = n.ReceiverId,
                Content = n.Content,
                CreatedAt = n.CreatedAt.GetValueOrDefault(),
                IsRead = n.IsRead.GetValueOrDefault()
            }).ToList();
        }

        public async Task MarkNotificationAsRead(int notificationId)
        {
            await _unitOfWork.BeginTransactionAsync();
            await _notificationRepository.MarkAsReadAsync(notificationId);
            await _unitOfWork.CommitAsync();
            await _unitOfWork.CommitTransactionAsync();
        }
    }
}

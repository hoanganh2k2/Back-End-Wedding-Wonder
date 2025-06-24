using BusinessObject.Models;
using DataAccess;
using Repository.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly NotificationDAO _dao;

        public NotificationRepository(NotificationDAO dao)
        {
            _dao = dao;
        }

        public Task AddNotificationAsync(Notification notification) => _dao.AddNotification(notification);

        public Task<List<Notification>> GetUnreadNotificationsAsync(int userId) => _dao.GetUserNotifications(userId);

        public Task MarkAsReadAsync(int notificationId) => _dao.MarkAsRead(notificationId);
    }
}

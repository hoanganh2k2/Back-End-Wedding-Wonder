using BusinessObject.Models;
using Repositories.IRepository;

namespace Repository.IRepositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetUserByEmail(string email);

        Task<User> GetUserByUserName(string username);

        Task DeleteImage(int id);

        Task<bool> UpgradeToSupplier(int userId);

        Task<bool> ChangePasswordAsync(int userId, string oldPassword, string newPassword);

        Task RecoverUser(int userId);

        Task<bool> VerifyAccount(string token);

        Task<bool> ForgotPassword(string email);

        Task<bool> ResetPassword(string token, string newPassword, string confirmPassword);

        Task<bool> CheckEmailExistsAsync(string email);

        Task<bool> IsUserOnline(int userId);

        Task UpdateUserOnlineStatus(int userId, bool isOnline);

        Task<IEnumerable<int>> GetServiceTopicsByServiceId(int serviceId);

        Task<bool> AddUserTopics(int userId, List<int> topicIds);
    }
}
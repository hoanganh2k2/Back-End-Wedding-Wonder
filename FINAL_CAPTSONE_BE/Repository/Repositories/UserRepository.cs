using BusinessObject.Models;
using DataAccess;
using Repository.IRepositories;

namespace Repository.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDAO _dao;

        public UserRepository(UserDAO dao)
        {
            _dao = dao;
        }

        public Task<bool> CreateAsync(User obj) => _dao.CreateUser(obj);

        public Task DeleteAsync(int id) => _dao.DeleteUser(id);

        public Task<User> GetAsyncById(int id) => _dao.GetUserById(id);

        public Task<List<User>> GetsAsync() => _dao.GetUsers();

        public Task UpdateAsync(int id, User obj) => _dao.UpdateUser(id, obj);

        public Task UpdateUserOnlineStatus(int userId, bool isOnline) => _dao.UpdateUserOnlineStatus(userId, isOnline);

        public Task<User> GetUserByEmail(string email) => _dao.GetUserByEmail(email);

        public Task<User> GetUserByUserName(string username) => _dao.GetUserByUserName(username);

        public Task<bool> UpgradeToSupplier(int userId) => _dao.UpgradeToSupplier(userId);

        public Task<bool> ChangePasswordAsync(int userId, string oldPassword, string newPassword) => _dao.ChangePasswordAsync(userId, oldPassword, newPassword);

        public Task RecoverUser(int userId) => _dao.RecoverUser(userId);

        public Task<bool> VerifyAccount(string token) => _dao.VerifyAccount(token);

        public Task<bool> ForgotPassword(string email) => _dao.ForgotPassword(email);

        public Task<bool> ResetPassword(string email, string newPassword, string confirmPassword) => _dao.ResetPassword(email, newPassword, confirmPassword);

        Task<bool> IUserRepository.CheckEmailExistsAsync(string email) => _dao.EmailExistsAsync(email);

        public Task DeleteImage(int id) => _dao.DeleteImage(id);

        public Task<bool> IsUserOnline(int userId) => _dao.IsUserOnline(userId);

        public Task<IEnumerable<int>> GetServiceTopicsByServiceId(int serviceId) => _dao.GetServiceTopicsByServiceId(serviceId);

        public async Task<bool> AddUserTopics(int userId, List<int> topicIds) => await _dao.AddUserTopics(userId, topicIds);
    }
}
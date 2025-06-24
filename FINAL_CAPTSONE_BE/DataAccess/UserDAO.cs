using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
namespace DataAccess
{
    public class UserDAO
    {
        private readonly WeddingWonderDbContext context;

        public UserDAO(WeddingWonderDbContext context)
        {
            this.context = context;
        }

        public async Task<List<User>> GetUsers()
        {
            try
            {
                return await context.Users
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<User> GetUserById(int userId)
        {
            try
            {
                User? userInDb = await context.Users
                    .FindAsync(userId);

                if (userInDb == null) throw new Exception("User not found");

                return userInDb;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<User> GetUserByEmail(string email)
        {
            try
            {
                return await context.Users
                    .FirstOrDefaultAsync(u => u.Email == email);
            }
            catch (Exception ex)
            {
                throw new Exception("Error from UserDAO(GetUserByEmail): " + ex.Message, ex);
            }
        }
        public async Task<User> GetUserByUserName(string username)
        {
            try
            {
                return await context.Users
                    .FirstOrDefaultAsync(u => u.UserName == username || u.Email == username);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<bool> CreateUser(User user)
        {
            try
            {
                bool isUsernameTaken = await context.Users.AnyAsync(u => u.UserName == user.UserName);
                if (isUsernameTaken) throw new Exception("Username is already taken.");

                bool isEmailTaken = await context.Users.AnyAsync(u => u.Email == user.Email);
                if (isEmailTaken) throw new Exception("Email is already registered.");

                if (user.PhoneNumber != null)
                {
                    bool isPhoneNumberTaken = await context.Users.AnyAsync(u => u.PhoneNumber == user.PhoneNumber);
                    if (isPhoneNumberTaken) throw new Exception("Phone number is already registered.");
                }

                await context.Users.AddAsync(user);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task DeleteImage(int id)
        {
            try
            {
                User? userInDb = await context.Users.FindAsync(id);
                if (userInDb == null) throw new Exception("User not found.");

                userInDb.UpdateAt = DateTime.Now;
                userInDb.UserImage = "https://quocbao.blob.core.windows.net/images/4/default_avata.jpg";

                context.Users.Update(userInDb);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task UpdateUser(int id, User user)
        {
            try
            {
                User? userInDb = await context.Users.FindAsync(id);
                if (userInDb == null) throw new Exception("User not found.");

                if (user.Email != userInDb.Email)
                {
                    bool emailExists = await context.Users.AnyAsync(u => u.Email == user.Email && u.UserId != id);
                    if (emailExists)
                    {
                        throw new Exception("The email address is already in use.");
                    }
                }

                if (user.PhoneNumber != userInDb.PhoneNumber)
                {
                    bool phoneExists = await context.Users.AnyAsync(u => u.PhoneNumber == user.PhoneNumber && u.UserId != id);
                    if (phoneExists)
                    {
                        throw new Exception("The phone number is already in use.");
                    }
                }

                userInDb.FullName = user.FullName;
                userInDb.PhoneNumber = user.PhoneNumber;
                userInDb.City = user.City;
                userInDb.District = user.District;
                userInDb.Ward = user.Ward;
                userInDb.Address = user.Address;
                userInDb.Dob = user.Dob;
                userInDb.Email = user.Email;
                userInDb.Gender = user.Gender;
                userInDb.UpdateAt = DateTime.Now;
                userInDb.Balance = user.Balance;

                context.Users.Update(userInDb);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task UpdateUserOnlineStatus(int userId, bool isOnline)
        {
            try
            {
                User? userInDb = await context.Users.FindAsync(userId);
                if (userInDb == null)
                    throw new Exception("User not found.");

                userInDb.IsOnline = isOnline;

                context.Users.Update(userInDb);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating user's online status: {ex.Message}", ex);
            }
        }

        public async Task DeleteUser(int userId)
        {
            try
            {
                User? userToDelete = await context.Users.FindAsync(userId);
                if (userToDelete != null)
                {
                    userToDelete.IsActive = false;
                }
                else
                {
                    throw new Exception("User not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task RecoverUser(int userId)
        {
            try
            {
                User? userToRecover = await context.Users.FindAsync(userId);
                if (userToRecover != null)
                {
                    userToRecover.IsActive = true;
                }
                else
                {
                    throw new Exception("User not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<bool> UpgradeToSupplier(int userId)
        {
            try
            {
                User? userToUpgrade = await context.Users.FindAsync(userId);
                if (userToUpgrade == null)
                {
                    throw new Exception("User not found.");
                }

                if (userToUpgrade.RoleId == 3)
                {
                    throw new Exception("User is already a supplier.");
                }
                else if (userToUpgrade.RoleId == 1 || userToUpgrade.RoleId == 2)
                {
                    throw new Exception("User cannot be upgraded due to the current role.");
                }

                userToUpgrade.RoleId = 3;
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<bool> ChangePasswordAsync(int userId, string oldPassword, string newPassword)
        {
            try
            {
                User? user = await context.Users.FindAsync(userId);
                if (user == null)
                {
                    throw new Exception("User not found");
                }

                bool isOldPasswordValid = BCrypt.Net.BCrypt.Verify(oldPassword, user.Password);
                if (!isOldPasswordValid)
                {
                    return false;
                }

                string hashedNewPassword = BCrypt.Net.BCrypt.HashPassword(newPassword, 12);
                user.Password = hashedNewPassword;

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<bool> VerifyAccount(string token)
        {
            try
            {
                Token? tokenInDb = await context.Tokens.FirstOrDefaultAsync(t => t.KeyValue == token && t.KeyName == "EmailToken");
                if (tokenInDb == null) throw new Exception("Invalid token");


                User? userInDb = await context.Users.FirstOrDefaultAsync(u => u.UserId == tokenInDb.UserId);
                if (userInDb == null) throw new Exception("User not found");
                userInDb.IsEmailConfirm = true;

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<bool> ForgotPassword(string email)
        {
            try
            {
                User? user = await context.Users.FirstOrDefaultAsync(u => u.Email == email && u.LoginProvider == "1");
                if (user == null)
                {
                    throw new Exception("No account found with this email");
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<bool> EmailExistsAsync(string email)
        {
            return await context.Users.AnyAsync(u => u.Email == email);
        }
        public async Task<bool> ResetPassword(string email, string newPassword, string confirmPassword)
        {
            try
            {
                User? user = await context.Users.FirstOrDefaultAsync(u => u.Email == email);
                if (user == null)
                {
                    throw new Exception("Invalid Token");
                }
                bool isNewPasswordSameAsOld = BCrypt.Net.BCrypt.Verify(newPassword, user.Password);
                if (isNewPasswordSameAsOld)
                {
                    throw new Exception("The new password cannot be the same as the old password");
                }
                string hashedNewPassword = BCrypt.Net.BCrypt.HashPassword(newPassword, 12);
                user.Password = hashedNewPassword;
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<bool> IsUserOnline(int userId)
        {
            try
            {
                User? userInDb = await context.Users.FindAsync(userId);
                if (userInDb == null) throw new Exception("User not found");

                return (bool)userInDb.IsOnline;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<IEnumerable<int>> GetServiceTopicsByServiceId(int serviceId)
        {
            try
            {
                return await context.ServiceTopics
                          .Where(st => st.ServiceId == serviceId)
                          .Select(st => st.TopicId)
                         .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error from UserDAO(GetServiceTopicsByServiceId): " + ex.Message, ex);
            }
        }
        public async Task<bool> AddUserTopics(int userId, List<int> topicIds)
        {
            try
            {
                foreach (int topicId in topicIds)
                {
                    UserTopic userTopic = new()
                    {
                        UserId = userId,
                        TopicId = topicId,
                        CreatedAt = DateTime.Now
                    };

                    await context.UserTopics.AddAsync(userTopic);
                }
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}

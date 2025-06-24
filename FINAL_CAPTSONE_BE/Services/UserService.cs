using BusinessObject.Contracts.IntergrationEvents;
using BusinessObject.DTOs;
using BusinessObject.Models;
using BusinessObject.Models.Requests;
using BusinessObject.Requests;
using eStoreAPI.Models;
using Google.Apis.Auth;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repository.IRepositories;
using Services.Email;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WeddingWonderAPI.Models.Response;

namespace Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenRepository _tokenRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMemoryCache _cache;
        private readonly IAdminLogRepository _adminLogRepository;
        private readonly AdminLogService _adminLogService;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly string _uploadsFolder;

        public UserService(IConfiguration configuration, IUserRepository userRepository, IUnitOfWork unitOfWork, ITokenRepository tokenRepository, IEmailService emailService, IHttpContextAccessor httpContextAccessor, IMemoryCache cache, IAdminLogRepository adminLogRepository, AdminLogService adminLogService, IPublishEndpoint publishEndpoint)
        {
            _configuration = configuration;
            _userRepository = userRepository;
            _tokenRepository = tokenRepository;
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _httpContextAccessor = httpContextAccessor;
            _cache = cache;
            _adminLogRepository = adminLogRepository;
            _adminLogService = adminLogService;
            _publishEndpoint = publishEndpoint;
            _uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Image");
        }

        public async Task<List<UserDTO>> GetsAsync()
        {
            try
            {
                List<User> users = await _userRepository.GetsAsync();

                List<UserDTO> userDTOs = users.Select(user => new UserDTO
                {
                    UserId = user.UserId,
                    FullName = user.FullName,
                    PhoneNumber = user.PhoneNumber,
                    City = user.City,
                    District = user.District,
                    Ward = user.Ward,
                    Address = user.Address,
                    Email = user.Email,
                    UserName = user.UserName,
                    Password = user.Password,
                    UserImage = user.UserImage,
                    Gender = user.Gender,
                    Dob = user.Dob,
                    FrontCmnd = user.FrontCmnd,
                    BackCmnd = user.BackCmnd,
                    CreatedAt = user.CreatedAt,
                    UpdateAt = user.UpdateAt,
                    RoleId = user.RoleId,
                    IsEmailConfirm = user.IsEmailConfirm,
                    IsActive = user.IsActive
                }).ToList();

                return userDTOs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<UserDTO> GetUserById(int userId)
        {
            try
            {
                User user = await _userRepository.GetAsyncById(userId);

                UserDTO userDTO = new()
                {
                    UserId = user.UserId,
                    FullName = user.FullName,
                    PhoneNumber = user.PhoneNumber,
                    City = user.City,
                    District = user.District,
                    Ward = user.Ward,
                    Address = user.Address,
                    Email = user.Email,
                    UserName = user.UserName,
                    Password = user.Password,
                    UserImage = user.UserImage,
                    Gender = user.Gender,
                    Dob = user.Dob,
                    FrontCmnd = user.FrontCmnd,
                    BackCmnd = user.BackCmnd,
                    CreatedAt = user.CreatedAt,
                    UpdateAt = user.UpdateAt,
                    RoleId = user.RoleId,
                    LoginProvider = user.LoginProvider,
                    IsEmailConfirm = user.IsEmailConfirm,
                    IsActive = user.IsActive,
                    IsOnline = user.IsOnline,
                    Balance = user.Balance,
                    IsUpgradeConfirmed = user.IsUpgradeConfirmed,
                    IsVipSupplier = (bool)user.IsVipSupplier
                };

                return userDTO;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<User> GetUserById2(int userId)
        {
            try
            {
                User user = await _userRepository.GetAsyncById(userId);

                User userDTO = new()
                {
                    UserId = user.UserId,
                    FullName = user.FullName,
                    PhoneNumber = user.PhoneNumber,
                    City = user.City,
                    District = user.District,
                    Ward = user.Ward,
                    Address = user.Address,
                    Email = user.Email,
                    UserName = user.UserName,
                    Password = user.Password,
                    UserImage = user.UserImage,
                    Gender = user.Gender,
                    Dob = user.Dob,
                    FrontCmnd = user.FrontCmnd,
                    BackCmnd = user.BackCmnd,
                    CreatedAt = user.CreatedAt,
                    UpdateAt = user.UpdateAt,
                    RoleId = user.RoleId,
                    LoginProvider = user.LoginProvider,
                    IsEmailConfirm = user.IsEmailConfirm,
                    IsActive = user.IsActive,
                    IsOnline = user.IsOnline,
                    Balance = user.Balance,
                    IsUpgradeConfirmed = user.IsUpgradeConfirmed,
                };

                return userDTO;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<UserDTO> GetUserByEmail(string email)
        {
            try
            {
                User user = await _userRepository.GetUserByEmail(email);

                UserDTO userDTO = new()
                {
                    UserId = user.UserId,
                    FullName = user.FullName,
                    PhoneNumber = user.PhoneNumber,
                    City = user.City,
                    District = user.District,
                    Ward = user.Ward,
                    Address = user.Address,
                    Email = user.Email,
                    UserName = user.UserName,
                    Password = user.Password,
                    UserImage = user.UserImage,
                    Gender = user.Gender,
                    Dob = user.Dob,
                    FrontCmnd = user.FrontCmnd,
                    BackCmnd = user.BackCmnd,
                    CreatedAt = user.CreatedAt,
                    UpdateAt = user.UpdateAt,
                    RoleId = user.RoleId,
                    LoginProvider = user.LoginProvider,
                    IsEmailConfirm = user.IsEmailConfirm,
                    IsActive = user.IsActive
                };

                return userDTO;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task DeleteUser(int userId, string reason, int adminId)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                await _userRepository.DeleteAsync(userId);

                User user = await _userRepository.GetAsyncById(userId);
                if (user != null)
                {
                    string subject = "Account Deactivation Notice";
                    string content = $"Dear {user.FullName},<br><br>Your account has been deactivated for the following reason:<br><br><strong>{reason}</strong><br><br>If you have any questions or wish to appeal this decision, please contact our support team.<br><br>Best regards,<br>Wedding Wonder Team";

                    EmailNotificationEvent emailEvent = new()
                    {
                        Id = Guid.NewGuid(),
                        ReceiverEmail = user.Email,
                        ReceiverName = user.FullName,
                        Subject = subject,
                        Body = content,
                        TimeStamp = DateTimeOffset.UtcNow
                    };

                    await _publishEndpoint.Publish(emailEvent);
                }
                string? ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress?.ToString();
                string userAgent = _httpContextAccessor.HttpContext.Request.Headers["User-Agent"].ToString();
                string deviceName = _adminLogService.ExtractDeviceName(userAgent);

                AdminLog adminLog = new()
                {
                    AdminId = adminId,
                    ActionType = "Delete",
                    DeviceType = deviceName,
                    IpAddress = ipAddress,
                    LogDetail = $"Deleted user account with ID {userId}",
                    CreatedAt = DateTime.Now
                };
                await _adminLogRepository.CreateAsync(adminLog);

                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task RecoverUser(int userId, int adminId)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                await _userRepository.RecoverUser(userId);
                string userAgent = _httpContextAccessor.HttpContext.Request.Headers["User-Agent"].ToString();
                string deviceName = _adminLogService.ExtractDeviceName(userAgent);
                string? ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress?.ToString();
                AdminLog adminLog = new()
                {
                    AdminId = adminId,
                    ActionType = "Recover",
                    DeviceType = deviceName,
                    IpAddress = ipAddress,
                    LogDetail = $"Recovered user account with ID {userId}",
                    CreatedAt = DateTime.Now
                };
                await _adminLogRepository.CreateAsync(adminLog);

                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task DeleteImage(int userId)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                await _userRepository.DeleteImage(userId);

                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<ApiResponse> UploadUserImageAsync(IFormFile file, int userId)
        {
            try
            {

                List<string> validExtensions = new() { ".jpg", ".png", ".gif" };
                string extension = Path.GetExtension(file.FileName).ToLower();

                if (!validExtensions.Contains(extension))
                {
                    return new ApiResponse
                    {
                        Status = false,
                        Message = $"Extension is not valid. Valid extensions are: {string.Join(", ", validExtensions)}"
                    };
                }


                long size = file.Length;
                if (size > 5 * 1024 * 1024)
                {
                    return new ApiResponse
                    {
                        Status = false,
                        Message = "Maximum size can be 5MB."
                    };
                }


                string fileName = Guid.NewGuid().ToString() + extension;

                string filePath = Path.Combine(_uploadsFolder, fileName);
                using (FileStream stream = new(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }


                User user = await _userRepository.GetAsyncById(userId);
                if (user == null)
                {
                    return new ApiResponse
                    {
                        Status = false,
                        Message = "User not found."
                    };
                }

                user.UserImage = fileName;


                _userRepository.UpdateAsync(userId, user);
                await _unitOfWork.CommitAsync();

                return new ApiResponse
                {
                    Status = true,
                    Message = "Image uploaded successfully."
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse
                {
                    Status = false,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }

        public async Task UpdateUser(int userId, UserDTO userDTO)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                User user = new()
                {
                    FullName = userDTO.FullName,
                    Dob = userDTO.Dob,
                    City = userDTO.City,
                    District = userDTO.District,
                    Ward = userDTO.Ward,
                    Address = userDTO.Address,
                    Email = userDTO.Email,
                    PhoneNumber = userDTO.PhoneNumber,
                    Gender = userDTO.Gender
                };
                await _userRepository.UpdateAsync(userId, user);

                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> UpgradeToSupplier(int userId)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                await _userRepository.UpgradeToSupplier(userId);

                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();

                return true;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> CreateUser(RegisterRequest userRequest)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                int costParameter = 12;
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(userRequest.Password, costParameter);

                User newUser = new()
                {
                    UserName = userRequest.UserName,
                    Password = hashedPassword,
                    PhoneNumber = userRequest.PhoneNumber,
                    FullName = userRequest.FullName,
                    Email = userRequest.Email,
                    Gender = userRequest.Gender,
                    UserImage = "https://quocbao.blob.core.windows.net/images/4/default_avata.jpg",
                    CreatedAt = DateTime.Now,
                    RoleId = 4,
                    IsEmailConfirm = false,
                    LoginProvider = "1",
                    IsActive = true,
                };
                await _userRepository.CreateAsync(newUser);
                await _unitOfWork.CommitAsync();
                string token = GenerateEmailToken();
                await _tokenRepository.SaveConfirmEmailToken(newUser.UserId, token, 1440); // 1day = 60x24

                await SendEmailConfirmationAsync(newUser.Email, token);

                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();

                return true;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception(ex.Message, ex);
            }
        }

        private async Task SendEmailConfirmationAsync(string email, string token)
        {
            string confirmationLink = GenerateEmailConfirmationLink(token);

            string subject = "Email Confirmation";
            string body = $"Please confirm your email by clicking on the link: <a href='{confirmationLink}'>Confirm Email</a>";

            EmailNotificationEvent emailEvent = new()
            {
                Id = Guid.NewGuid(),
                ReceiverEmail = email,
                ReceiverName = "User",
                Subject = subject,
                Body = body,
                TimeStamp = DateTimeOffset.Now
            };

            await _publishEndpoint.Publish(emailEvent);
        }

        private string GenerateEmailConfirmationLink(string token)
        {
            HttpRequest request = _httpContextAccessor.HttpContext.Request;
            string scheme = request.Scheme;
            string host = request.Host.ToString();

            string confirmationLink = $"{scheme}://{host}/api/user/confirmEmail?token={token}";

            return confirmationLink;
        }

        private string GenerateEmailToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }

        public async Task SetUserOnlineStatus(int userId, bool isOnline)
        {
            try
            {
                await _userRepository.UpdateUserOnlineStatus(userId, isOnline);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the user's online status", ex);
            }
        }

        public async Task<User> LoginByGoogle(LoginGoogleRequest request)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                if (string.IsNullOrEmpty(request.IdToken))
                {
                    throw new ArgumentException("IdToken is required.", nameof(request.IdToken));
                }

                GoogleJsonWebSignature.Payload payload = await ValidateGoogleToken(request.IdToken);
                if (payload == null)
                {
                    throw new InvalidOperationException("Invalid Google token");
                }

                User? user = await _userRepository.GetUserByUserName(payload.Email);

                if (user == null)
                {
                    user = new User
                    {
                        UserName = payload.Email,
                        Email = payload.Email,
                        UserImage = payload.Picture,
                        FullName = payload.Name,
                        IsActive = true,
                        CreatedAt = DateTime.Now,
                        RoleId = 4,
                        Gender = 1,
                        LoginProvider = "2",
                        IsEmailConfirm = true
                    };
                    string randomPassword = GenerateRandomPassword();

                    user.Password = BCrypt.Net.BCrypt.HashPassword(randomPassword);
                    await _userRepository.CreateAsync(user);
                }
                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
                return user;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<User> LoginByAccount(LoginRequest loginRequest)
        {
            try
            {
                User? userInDb = await _userRepository.GetUserByUserName(loginRequest.Username);

                if (userInDb == null || !BCrypt.Net.BCrypt.Verify(loginRequest.Password, userInDb.Password))
                    throw new Exception("Invalid Username or Password");

                if (userInDb.IsActive == false)
                    throw new Exception("This Account has been deactivated");

                if (userInDb.IsEmailConfirm == false)
                    throw new Exception("Email unverified account");

                return userInDb;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> ChangePassword(int userId, string oldPassword, string newPassword)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                bool status = await _userRepository.ChangePasswordAsync(userId, oldPassword, newPassword);

                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();

                return status;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> Verify(string token)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                await _userRepository.VerifyAccount(token);

                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();

                return true;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<Token> GetTokenByUserId(int userId, string keyName)
        {
            try
            {
                return await _tokenRepository.GetTokenById(userId, keyName);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task SaveRefreshToken(int userId, string refreshToken)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                await _tokenRepository.SaveRefreshToken(userId, refreshToken, 10080); // 7 days = 60 * 24 * 7

                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception(ex.Message, ex);
            }
        }

        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            TokenValidationParameters tokenValidationParameters = new()
            {
                ValidateAudience = false,
                ValidateIssuer = false,

                ValidAudience = _configuration["JWT:Audience"],
                ValidIssuer = _configuration["JWT:Issuer"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"])),
                ValidateLifetime = false,
                ClockSkew = TimeSpan.Zero
            };

            JwtSecurityTokenHandler tokenHandler = new();
            try
            {
                ClaimsPrincipal principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken? securityToken);
                JwtSecurityToken? jwtSecurityToken = securityToken as JwtSecurityToken;
                if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new SecurityTokenException("Invalid token");
                }
                return principal;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while validating the token.", ex);
            }
        }

        public JwtSecurityToken GetTokenOfLogin(User user)
        {
            List<Claim> authClaims = new()
                {
                    new Claim(ClaimTypes.Name, user.FullName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("Id", user.UserId.ToString()),
                    new Claim("LoginProvider", "1"),
                    new Claim("IsUpgradeConfirmed", user.IsUpgradeConfirmed.ToString())
                };

            switch (user.RoleId)
            {
                case 1:
                    authClaims.Add(new Claim(ClaimTypes.Role, UserRoles.SuperAdmin));
                    break;

                case 2:
                    authClaims.Add(new Claim(ClaimTypes.Role, UserRoles.Admin));
                    break;

                case 3:
                    authClaims.Add(new Claim(ClaimTypes.Role, UserRoles.Supplier));
                    break;

                default:
                    authClaims.Add(new Claim(ClaimTypes.Role, UserRoles.Customer));
                    break;
            }

            return GetToken(authClaims);
        }

        public JwtSecurityToken GetTokenOfLoginGoogle(User user)
        {
            List<Claim> authClaims = new()
                {
                    new Claim(ClaimTypes.Name, user.FullName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("Id", user.UserId.ToString()),
                    new Claim("LoginProvider", "2"),
                };

            switch (user.RoleId)
            {
                case 1:
                    authClaims.Add(new Claim(ClaimTypes.Role, UserRoles.SuperAdmin));
                    break;

                case 2:
                    authClaims.Add(new Claim(ClaimTypes.Role, UserRoles.Admin));
                    break;

                case 3:
                    authClaims.Add(new Claim(ClaimTypes.Role, UserRoles.Supplier));
                    break;

                default:
                    authClaims.Add(new Claim(ClaimTypes.Role, UserRoles.Customer));
                    break;
            }

            return GetToken(authClaims);
        }

        public JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            SymmetricSecurityKey authSigningKey = new(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            JwtSecurityToken token = new(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                expires: DateTime.Now.AddHours(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

        public string GenerateRefreshToken()
        {
            byte[] randomNumber = new byte[32];
            using RandomNumberGenerator rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private async Task<GoogleJsonWebSignature.Payload> ValidateGoogleToken(string idToken)
        {
            try
            {
                string clientId = _configuration["GoogleKeys:ClientId"];
                if (string.IsNullOrEmpty(clientId))
                {
                    throw new InvalidOperationException("ClientId is not configured.");
                }

                GoogleJsonWebSignature.ValidationSettings settings = new()
                {
                    Audience = new[] { clientId }
                };

                GoogleJsonWebSignature.Payload payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
                Console.WriteLine($"Token Payload: {payload}");
                Console.WriteLine($"Token Audience: {payload.Audience}");

                return payload;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error validating Google token: {ex.Message}");
                throw new Exception("Failed to validate Google token.", ex);
            }
        }

        public static string GenerateRandomPassword(int length = 50)
        {
            if (length < 8)
            {
                throw new ArgumentException("Password length should be at least 8 characters.");
            }

            const string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*?_-";
            StringBuilder password = new(length);

            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                byte[] randomNumber = new byte[1];
                for (int i = 0; i < length; i++)
                {
                    rng.GetBytes(randomNumber);
                    int index = randomNumber[0] % validChars.Length;
                    password.Append(validChars[index]);
                }
            }

            return password.ToString();
        }

        public async Task SendOTPAsync(string email)
        {
            try
            {
                string otpCode = GenerateOTP();
                DateTime expirationTime = DateTime.Now.AddMinutes(5);

                string subject = "Your OTP Code";
                string content = $"Your OTP code is {otpCode}. It will expire in 5 minutes.";

                _cache.Set(email + "_OTP", otpCode, expirationTime - DateTime.Now);
                _cache.Set(email + "_OTPExpiration", expirationTime);

                bool resetSuccessful = await CheckEmailExistsAsync(email);
                if (!resetSuccessful)
                {
                    throw new Exception("An error occurred while sending data");
                }

                EmailNotificationEvent emailEvent = new()
                {
                    Id = Guid.NewGuid(),
                    ReceiverEmail = email,
                    ReceiverName = "User",
                    Subject = subject,
                    Body = content,
                    TimeStamp = DateTimeOffset.UtcNow
                };

                await _publishEndpoint.Publish(emailEvent);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while sending the OTP", ex);
            }
        }

        private async Task<bool> CheckEmailExistsAsync(string email)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                bool emailExists = await _userRepository.ForgotPassword(email);

                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();

                return emailExists;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while sending data", ex);
            }
        }

        public async Task<bool> ResetPassword(string email, string password, string confirmPassword)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                await _userRepository.ResetPassword(email, password, confirmPassword);
                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while sending data", ex);
            }
        }

        public bool VerifyOTP(string email, string otpCode)
        {
            string? cachedOtp = _cache.Get<string>(email + "_OTP");
            DateTime? cachedExpiration = _cache.Get<DateTime?>(email + "_OTPExpiration");

            if (cachedOtp == null || cachedExpiration == null)
            {
                return false;
            }

            return cachedOtp == otpCode && cachedExpiration > DateTime.Now;
        }

        public string GenerateOTP(int length = 6)
        {
            Random random = new();
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public async Task<bool> GetUserOnlineStatusAsync(int userId)
        {
            return await _userRepository.IsUserOnline(userId);
        }

        public async Task<bool> AddUserTopicsAsync(int userId, List<int> topicIds)
        {
            try
            {
                return await _userRepository.AddUserTopics(userId, topicIds);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task UpdateUserBalance(int userId, double amount)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                User user = await _userRepository.GetAsyncById(userId);
                if (user != null)
                {

                    user.Balance = (user.Balance ?? 0) - amount;
                    user.IsVipSupplier = true;
                    await _userRepository.UpdateAsync(userId, user);

                    await _unitOfWork.CommitAsync();
                    await _unitOfWork.CommitTransactionAsync();
                }
                else
                {
                    throw new Exception("User not found.");
                }
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
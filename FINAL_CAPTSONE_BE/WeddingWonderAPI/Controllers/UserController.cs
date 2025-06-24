using BusinessObject.DTOs;
using BusinessObject.Models;
using BusinessObject.Models.Requests;
using BusinessObject.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WeddingWonderAPI.Helper;
using WeddingWonderAPI.Models.DTOs;
using WeddingWonderAPI.Models.Response;

namespace WeddingWonderAPI.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly UserService _userService;
        public UserController(IConfiguration configuration, UserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }

        [HttpGet("allUsers")]
        [Authorize(Roles = "Admin, Super Admin")]
        public async Task<ActionResult> GetUsers()
        {
            try
            {
                List<UserDTO> users = await _userService.GetsAsync();
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = $"Get list users successfully",
                    Data = users
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Status = false,
                    Message = $"{ex.Message}"
                });
            }
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult> GetUserById(int id)
        {
            try
            {
                UserDTO user = await _userService.GetUserById(id);
                if (user == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Status = false,
                        Message = "User not found"
                    });
                }

                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Get Information User Success",
                    Data = user,
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Status = false,
                    Message = $"{ex.Message}"
                });
            }
        } 

        [HttpGet("email/{email}")]
        public async Task<ActionResult> GetUserByEmail(string email)
        {
            try
            {
                UserDTO user = await _userService.GetUserByEmail(email);
                if (user == null)
                {
                    return NotFound(new ApiResponse
                    {
                        Status = false,
                        Message = "User not found"
                    });
                }

                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Get Information User Success",
                    Data = user,
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Status = false,
                    Message = $"{ex.Message}"
                });
            }
        }
        [HttpDelete("deleteByNotAdmin")]
        [Authorize(Roles = "Customer, Supplier")]
        public async Task<IActionResult> DeleteByNotAdmin()
        {
            try
            {
                int userId = UserHelper.GetUserId(User);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "User has been deactivated"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Status = false,
                    Message = $"{ex.Message}"
                });
            }
        }

        [HttpDelete("deleteByAdmin/{id:int}")]
        [Authorize(Roles = "Admin, Super Admin")]
        public async Task<IActionResult> DeleteByAdmin(int id, [FromQuery] string reason)
        {
            try
            {
                int userId = UserHelper.GetUserId(User);
                await _userService.DeleteUser(id, reason, userId);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "User has been deactivated"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Status = false,
                    Message = $"{ex.Message}"
                });
            }
        }

        [HttpPut("recover/{id:int}")]
        [Authorize(Roles = "Admin, Super Admin")]
        public async Task<IActionResult> Recover(int id)
        {
            try
            {
                int userId = UserHelper.GetUserId(User);
                await _userService.RecoverUser(id, userId);

                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "User has been activated"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Status = false,
                    Message = $"{ex.Message}"
                });
            }
        }
        [HttpPut("deleteImage")]
        [Authorize]
        public async Task<IActionResult> DeleteImage()
        {
            try
            {
                int userId = UserHelper.GetUserId(User);
                await _userService.DeleteImage(userId);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Information updated successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Status = false,
                    Message = $"{ex.Message}"
                });
            }
        }
        [HttpPut("updateInfor")]
        [Authorize]
        public async Task<IActionResult> Put([FromBody] UserDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse
                {
                    Status = false,
                    Message = "Invalid data",
                });
            }

            try
            {
                int userId = UserHelper.GetUserId(User);
                await _userService.UpdateUser(userId, userDTO);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Information updated successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Status = false,
                    Message = $"{ex.Message}"
                });
            }
        }

        [HttpPut("upgradeToSupplier/{id}")]
        [Authorize(Roles = "Admin, Super Admin")]
        public async Task<ActionResult> UpgradeToSupplier(int id)
        {
            try
            {
                bool status = await _userService.UpgradeToSupplier(id);
                if (status)
                {
                    return Ok(new ApiResponse
                    {
                        Status = true,
                        Message = "Successfully upgraded account to supplier."
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Status = false,
                    Message = $"{ex.Message}"
                });
            }

            return BadRequest(new ApiResponse
            {
                Status = false,
                Message = "An error occurred when upgrading account to supplier."
            });
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest userRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse
                {
                    Status = false,
                    Message = "Invalid data",
                });
            }
            try
            {
                bool status = await _userService.CreateUser(userRequest);
                if (status)
                {
                    return Ok(new ApiResponse
                    {
                        Status = true,
                        Message = "User registered successfully"
                    });
                }
                else
                {
                    return BadRequest(new ApiResponse
                    {
                        Status = false,
                        Message = "Failed to register user"
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Status = false,
                    Message = $"{ex.Message}"
                });
            }
        }

        [HttpPost]
        [Route("loginByAccount")]
        public async Task<IActionResult> LoginByAccount([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse
                {
                    Status = false,
                    Message = "Invalid data",
                });
            }
            try
            {
                User user = await _userService.LoginByAccount(request);

                JwtSecurityToken token = _userService.GetTokenOfLogin(user);
                string accessToken = new JwtSecurityTokenHandler().WriteToken(token);

                string refreshToken = _userService.GenerateRefreshToken();
                await _userService.SaveRefreshToken(user.UserId, refreshToken);

                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Authenticate success",
                    Data = new
                    {
                        AccessToken = accessToken,
                        RefreshToken = refreshToken,
                        RoleId = user.RoleId,
                        IsUpgradeConfirmed = user.IsUpgradeConfirmed
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Status = false,
                    Message = $"{ex.Message}"
                });
            }
        }
        [HttpPost]
        [Route("loginGoogle")]
        public async Task<IActionResult> LoginByGoogle([FromBody] LoginGoogleRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse
                {
                    Status = false,
                    Message = "Invalid data",
                });
            }
            try
            {
                User user = await _userService.LoginByGoogle(request);

                JwtSecurityToken token = _userService.GetTokenOfLoginGoogle(user);
                string accessToken = new JwtSecurityTokenHandler().WriteToken(token);

                string refreshToken = _userService.GenerateRefreshToken();
                await _userService.SaveRefreshToken(user.UserId, refreshToken);

                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Authenticate success",
                    Data = new
                    {
                        AccessToken = accessToken,
                        RefreshToken = refreshToken
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Status = false,
                    Message = $"{ex.Message}"
                });
            }
        }
        [HttpPut("changePass")]
        [Authorize]
        public async Task<IActionResult> ChangePassWord([FromBody] ChangePassRequest passRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse
                {
                    Status = false,
                    Message = "Invalid data"
                });
            }

            if (passRequest.NewPassword != passRequest.ConfirmNewPassword)
            {
                return BadRequest(new ApiResponse
                {
                    Status = false,
                    Message = "New Password and Confirm New Password do not match"
                });
            }

            try
            {
                int userId = UserHelper.GetUserId(User);
                bool status = await _userService.ChangePassword(userId, passRequest.OldPassword, passRequest.NewPassword);
                if (status)
                {
                    return Ok(new ApiResponse
                    {
                        Status = true,
                        Message = "Password changed successfully"
                    });
                }
                else
                {
                    return BadRequest(new ApiResponse
                    {
                        Status = false,
                        Message = "Old Password is incorrect"
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Status = false,
                    Message = $"{ex.Message}"
                });
            }
        }
        [HttpGet("confirmEmail")]
        public async Task<ActionResult> ConfirmEmail([FromQuery] string token)
        {
            try
            {
                bool status = await _userService.Verify(token);
                if (status)
                {
                    return Ok(new ApiResponse
                    {
                        Status = true,
                        Message = "Email verified successfully"
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Status = false,
                    Message = $"{ex.Message}"
                });
            }

            return BadRequest(new ApiResponse
            {
                Status = false,
                Message = "Email verification failed"
            });
        }
        [HttpPost("send-otp")]
        public async Task<IActionResult> SendOtpToEmail([FromBody] ForgotPasswordRequest request)
        {
            try
            {
                await _userService.SendOTPAsync(request.Email);

                return Ok(new { message = "Mã OTP đã được gửi đến email của bạn." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPost("verify-otp")]
        public IActionResult VerifyOTP([FromBody] VerifyOTPRequest request)
        {
            try
            {
                bool isOtpValid = _userService.VerifyOTP(request.Email, request.OTP);

                if (isOtpValid)
                {
                    return Ok(new { Message = "OTP is valid." });
                }
                else
                {
                    return BadRequest(new { Message = "Invalid or expired OTP." });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPasswordAsync([FromBody] ResetPasswordRequest request)
        {
            try
            {
                if (request.Password != request.ConfirmPassword)
                {
                    return BadRequest(new { Message = "Password and Confirm Password do not match." });
                }

                bool resetSuccessful = await _userService.ResetPassword(request.Email, request.Password, request.ConfirmPassword);

                if (!resetSuccessful)
                {
                    return BadRequest(new { Message = "Invalid or expired token." });
                }

                return Ok(new { Message = "Password has been reset successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
        [HttpPost]
        [Route("newAccessToken")]
        public async Task<IActionResult> NewAccessToken([FromBody] TokenRequest request)
        {
            try
            {
                if (request == null || string.IsNullOrEmpty(request.AccessToken) || string.IsNullOrEmpty(request.RefreshToken))
                {
                    return BadRequest(new ApiResponse
                    {
                        Status = false,
                        Message = "Invalid client request"
                    });
                }
                ClaimsPrincipal? principal = _userService.GetPrincipalFromExpiredToken(request.AccessToken);
                if (principal == null)
                {
                    return BadRequest(new ApiResponse
                    {
                        Status = false,
                        Message = "Invalid access token or refresh token"
                    });
                }

                int userId = int.Parse(principal.FindFirst("Id")?.Value);
                User user = await _userService.GetUserById2(userId);
                Token savedRefreshToken = await _userService.GetTokenByUserId(userId, "RefreshToken");

                if (savedRefreshToken.KeyValue != request.RefreshToken)
                {
                    return BadRequest(new ApiResponse
                    {
                        Status = false,
                        Message = "Invalid refresh token"
                    });
                }

                if (DateTime.Now > savedRefreshToken.Expiration)
                {
                    return BadRequest(new ApiResponse
                    {
                        Status = false,
                        Message = "Refresh token expired"
                    });
                }

                string newAccessToken = new JwtSecurityTokenHandler().WriteToken(_userService.GetTokenOfLogin(user));
                string newRefreshToken = _userService.GenerateRefreshToken();
                await _userService.SaveRefreshToken(userId, newRefreshToken);

                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "Token refreshed successfully",
                    Data = new
                    {
                        AccessToken = newAccessToken,
                        RefreshToken = newRefreshToken
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Status = false,
                    Message = $"{ex.Message}"
                });
            }
        }
        [HttpPut("setOnline")]
        [Authorize]
        public async Task<IActionResult> SetUserOnline()
        {
            try
            {
                int userId = UserHelper.GetUserId(User);
                await _userService.SetUserOnlineStatus(userId, true);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "User is now online"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Status = false,
                    Message = $"{ex.Message}"
                });
            }
        }

        [HttpPut("setOffline")]
        [Authorize]
        public async Task<IActionResult> SetUserOffline()
        {
            try
            {
                int userId = UserHelper.GetUserId(User);
                await _userService.SetUserOnlineStatus(userId, false);
                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "User is now offline"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Status = false,
                    Message = $"{ex.Message}"
                });
            }
        }
        [HttpGet("isOnline/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetUserOnlineStatus(int userId)
        {
            try
            {
                bool isOnline = await _userService.GetUserOnlineStatusAsync(userId);

                return Ok(new ApiResponse
                {
                    Status = true,
                    Message = "User online status retrieved successfully",
                    Data = new { IsOnline = isOnline }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Status = false,
                    Message = ex.Message,
                    Data = new { IsOnline = false }
                });
            }
        }
        [HttpPost("/addTopics")]
        [Authorize]
        public async Task<IActionResult> AddUserTopics( [FromBody] AddUserTopicsRequest request)
{
         try
              {
                int userId = UserHelper.GetUserId(User);
                await _userService.SetUserOnlineStatus(userId, false);
                bool status = await _userService.AddUserTopicsAsync(userId, request.TopicIds);
             if (status)
                 {
                return Ok(new ApiResponse
                   {
                      Status = true,
                      Message = "Topics added to user successfully"
                   });
                 }
               return BadRequest(new ApiResponse
                 {
                  Status = false,
                  Message = "Failed to add topics to user"
                 });
            }
            catch (Exception ex)
           {
             return BadRequest(new ApiResponse
           {
            Status = false,
            Message = ex.Message
           });
    }

}
        [HttpPost("upload-image/{userId}")]
    
        public async Task<ActionResult> UploadUserImage(int userId, IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest(new ApiResponse
                    {
                        Status = false,
                        Message = "No file uploaded."
                    });
                }

                var result = await _userService.UploadUserImageAsync(file, userId);

                if (result.Status.HasValue && result.Status.Value) // Kiểm tra nếu Status là true
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Status = false,
                    Message = $"An error occurred: {ex.Message}"
                });
            }
        }

        [HttpPost("upgrade-vip")]
        [Authorize(Roles = "Supplier")]
        public async Task<IActionResult> PayOrUpgradeVip()
        {
            int userId = UserHelper.GetUserId(User);

            var user = await _userService.GetUserById(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            if (user.Balance >= 100000)
            {
                await _userService.UpdateUserBalance(userId, 100000);
                return Ok(new
                {
                    Success = true,
                    Message = "Successful. You now vip supplier."
                });
            }
            else
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = "Not enough money . Please deposite"
                });

            }
        }
    }
}

using System.Security.Claims;

namespace WeddingWonderAPI.Helper
{
    public static class UserHelper
    {
        public static int GetUserId(ClaimsPrincipal user)
        {
            Claim? userIdClaim = user.FindFirst("Id");
            if (userIdClaim == null)
            {
                throw new UnauthorizedAccessException("User is not authorized");
            }

            if (!int.TryParse(userIdClaim.Value, out int userId))
            {
                throw new UnauthorizedAccessException("Invalid user ID");
            }

            return userId;
        }
        public static string GetRole(ClaimsPrincipal user)
        {
            string? userRole = user.FindFirstValue(ClaimTypes.Role);
            if (userRole == null)
            {
                throw new UnauthorizedAccessException("User is not authorized");
            }

            return userRole;
        }

         public static int GetUserIdForService(ClaimsPrincipal user)
        {
            Claim? userIdClaim = user.FindFirst("Id");
            if (userIdClaim == null)
            {
                return 0;
            }

            if (!int.TryParse(userIdClaim.Value, out int userId))
            {
                return 0;
            }

            return userId;
        }
        public static string GetRoleForService(ClaimsPrincipal user)
        {
            string? userRole = user.FindFirstValue(ClaimTypes.Role);
            if (userRole == null)
            {
               return "guest";
            }

            return userRole;
        }
        public static string GetUserEmail(ClaimsPrincipal user)
{
    string? userEmail = user.FindFirstValue(ClaimTypes.Email);
    if (userEmail == null)
    {
        throw new UnauthorizedAccessException("User email is not available");
    }

    return userEmail;
}

public static string GetUserName(ClaimsPrincipal user)
{
    string? userName = user.FindFirstValue(ClaimTypes.Name);
    if (userName == null)
    {
        throw new UnauthorizedAccessException("User name is not available");
    }

    return userName;
}
    }
    
}

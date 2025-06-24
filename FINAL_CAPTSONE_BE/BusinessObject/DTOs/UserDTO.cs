namespace BusinessObject.DTOs
{
    public class UserDTO
    {
        public int? UserId { get; set; }
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }

        public string? City { get; set; }

        public string? District { get; set; }

        public string? Ward { get; set; }

        public string? Address { get; set; }
        public string? Email { get; set; }

        public string? UserName { get; set; } = null!;

        public string? Password { get; set; } = null!;

        public string? UserImage { get; set; }

        public int? Gender { get; set; }
        public DateTime? Dob { get; set; }

        public string? FrontCmnd { get; set; }

        public string? BackCmnd { get; set; }

        public int RoleId { get; set; }

        public int? IsUpgradeConfirmed { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdateAt { get; set; }

        public string? LoginProvider { get; set; }
        public bool? IsEmailConfirm { get; set; }
        public bool? IsActive { get; set; }

        public bool? IsOnline { get; set; }

        public int ConversationId { get; set; }

        public double? Balance { get; set; }
        public bool IsVipSupplier { get; set; }
    }
}

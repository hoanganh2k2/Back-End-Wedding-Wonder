using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class User
{
    public int UserId { get; set; }

    public string? FullName { get; set; }

    public string? PhoneNumber { get; set; }

    public string? City { get; set; }

    public string? District { get; set; }

    public string? Ward { get; set; }

    public string? Address { get; set; }

    public string? Email { get; set; }

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? UserImage { get; set; }

    public int? Gender { get; set; }

    public DateTime? Dob { get; set; }

    public string? FrontCmnd { get; set; }

    public string? BackCmnd { get; set; }

    public int RoleId { get; set; }

    public int? IsUpgradeConfirmed { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public bool? IsVipSupplier { get; set; }

    public string? LoginProvider { get; set; }

    public bool? IsEmailConfirm { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsOnline { get; set; }

    public double? Balance { get; set; }

    public virtual ICollection<AdminLog> AdminLogs { get; } = new List<AdminLog>();

    public virtual ICollection<ComboBooking> ComboBookings { get; } = new List<ComboBooking>();

    public virtual ICollection<Contractbooking> Contractbookings { get; } = new List<Contractbooking>();

    public virtual ICollection<Contract> Contracts { get; } = new List<Contract>();

    public virtual ICollection<CustomerReview> CustomerReviews { get; } = new List<CustomerReview>();

    public virtual ICollection<FavoriteService> FavoriteServices { get; } = new List<FavoriteService>();

    public virtual ICollection<Message> MessageReceivers { get; } = new List<Message>();

    public virtual ICollection<Message> MessageSenders { get; } = new List<Message>();

    public virtual ICollection<Notification> Notifications { get; } = new List<Notification>();

    public virtual ICollection<RequestUpgradeSupplier> RequestUpgradeSuppliers { get; } = new List<RequestUpgradeSupplier>();

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<Service> Services { get; } = new List<Service>();

    public virtual ICollection<SingleBooking> SingleBookings { get; } = new List<SingleBooking>();

    public virtual ICollection<Token> Tokens { get; } = new List<Token>();

    public virtual ICollection<UserTopic> UserTopics { get; } = new List<UserTopic>();
}

using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class Notification
{
    public int Id { get; set; }

    public int ReceiverId { get; set; }

    public string Content { get; set; } = null!;

    public int? BookingId { get; set; }

    public int NotificationType { get; set; }

    public int IsSigned { get; set; }

    public DateTime? CreatedAt { get; set; }

    public bool? IsRead { get; set; }

    public virtual User Receiver { get; set; } = null!;
}

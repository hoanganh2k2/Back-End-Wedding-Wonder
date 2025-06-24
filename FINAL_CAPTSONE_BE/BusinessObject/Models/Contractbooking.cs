using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class Contractbooking
{
    public int ContractBookingId { get; set; }

    public int BookingId { get; set; }

    public int UserId { get; set; }

    public int ServiceId { get; set; }

    public DateTime CreatedDate { get; set; }

    public decimal? Subtotal { get; set; }

    public decimal? Total { get; set; }

    public decimal? Discount { get; set; }

    public bool? IsSigned { get; set; }

    public string? Signature { get; set; }

    public virtual ComboBooking Booking { get; set; } = null!;

    public virtual Service Service { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}

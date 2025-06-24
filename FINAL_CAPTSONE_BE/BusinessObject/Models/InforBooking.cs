using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class InforBooking
{
    public int InforId { get; set; }

    public string FullName { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string City { get; set; } = null!;

    public string District { get; set; } = null!;

    public string Ward { get; set; } = null!;

    public string Address { get; set; } = null!;

    public DateTime? DateOfBirth { get; set; }

    public virtual ICollection<ComboBooking> ComboBookingInforBrides { get; } = new List<ComboBooking>();

    public virtual ICollection<ComboBooking> ComboBookingInforGrooms { get; } = new List<ComboBooking>();

    public virtual ICollection<SingleBooking> SingleBookings { get; } = new List<SingleBooking>();
}

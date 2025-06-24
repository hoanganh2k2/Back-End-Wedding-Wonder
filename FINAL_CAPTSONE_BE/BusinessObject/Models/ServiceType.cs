using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class ServiceType
{
    public int ServiceTypeId { get; set; }

    public string? ServiceTypeName { get; set; }

    public virtual ICollection<BookingServiceDetail> BookingServiceDetails { get; } = new List<BookingServiceDetail>();

    public virtual ICollection<Service> Services { get; } = new List<Service>();

    public virtual ICollection<SingleBooking> SingleBookings { get; } = new List<SingleBooking>();
}

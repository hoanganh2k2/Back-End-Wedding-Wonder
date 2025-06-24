using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class BookingServiceDetail
{
    public int DetailId { get; set; }

    public int ServiceId { get; set; }

    public int ServiceTypeId { get; set; }

    public int? PrePackageId { get; set; }

    public int? PackageId { get; set; }

    public decimal TotalPrice { get; set; }

    public decimal BasicPrice { get; set; }

    public int BookingId { get; set; }

    public string? Note { get; set; }

    public int? NumberOfUses { get; set; }

    public DateTime? PreAppointment { get; set; }

    public DateTime? Appointment { get; set; }

    public int? ArtistId { get; set; }

    public bool ServiceMode { get; set; }

    public bool Priority { get; set; }

    public int Status { get; set; }

    public virtual MakeUpArtist? Artist { get; set; }

    public virtual ComboBooking Booking { get; set; } = null!;

    public virtual Service Service { get; set; } = null!;

    public virtual ServiceType ServiceType { get; set; } = null!;
}

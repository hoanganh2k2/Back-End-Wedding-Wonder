using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class SingleBooking
{
    public int BookingId { get; set; }

    public DateTime DateOfUse { get; set; }

    public DateTime CreatedAt { get; set; }

    public int UserId { get; set; }

    public int ServiceId { get; set; }

    public int ServiceTypeId { get; set; }

    public decimal BasicPrice { get; set; }

    public decimal TotalPrice { get; set; }

    public int? PackageId { get; set; }

    public int? NumberOfUses { get; set; }

    public int InforId { get; set; }

    public int? ArtistId { get; set; }

    public bool ServiceMode { get; set; }

    public string? Note { get; set; }

    public int BookingStatus { get; set; }

    public virtual MakeUpArtist? Artist { get; set; }

    public virtual InforBooking Infor { get; set; } = null!;

    public virtual Service Service { get; set; } = null!;

    public virtual ServiceType ServiceType { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}

using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class MakeUpArtist
{
    public int ArtistId { get; set; }

    public string ArtistName { get; set; } = null!;

    public string? ArtistImage { get; set; }

    public int Experience { get; set; }

    public string? Services { get; set; }

    public string? Certifications { get; set; }

    public string? Awards { get; set; }

    public int ServiceId { get; set; }

    public bool? Status { get; set; }

    public decimal? ProfessionalFee { get; set; }

    public virtual ICollection<BookingServiceDetail> BookingServiceDetails { get; } = new List<BookingServiceDetail>();

    public virtual MakeUpService Service { get; set; } = null!;

    public virtual ICollection<SingleBooking> SingleBookings { get; } = new List<SingleBooking>();
}

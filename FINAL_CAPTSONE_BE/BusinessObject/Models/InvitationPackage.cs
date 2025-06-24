using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class InvitationPackage
{
    public int PackageId { get; set; }

    public string PackageName { get; set; } = null!;

    public decimal? PackagePrice { get; set; }

    public string? PackageDescribe { get; set; }

    public string? Envelope { get; set; }

    public string? Component { get; set; }

    public string? Size { get; set; }

    public int ServiceId { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<InvitationImage> InvitationImages { get; } = new List<InvitationImage>();

    public virtual InvitationService Service { get; set; } = null!;
}

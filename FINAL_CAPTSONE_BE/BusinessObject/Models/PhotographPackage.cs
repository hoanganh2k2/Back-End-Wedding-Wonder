using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class PhotographPackage
{
    public int PackageId { get; set; }

    public string PackageName { get; set; } = null!;

    public decimal? PackagePrice { get; set; }

    public string? PackageContent { get; set; }

    public string? WorkFlow { get; set; }

    public int ServiceId { get; set; }

    public int EventType { get; set; }

    public int PhotoType { get; set; }

    public string Location { get; set; } = null!;

    public string? ImageSamples { get; set; }

    public bool? Status { get; set; }

    public virtual PhotographService Service { get; set; } = null!;
}

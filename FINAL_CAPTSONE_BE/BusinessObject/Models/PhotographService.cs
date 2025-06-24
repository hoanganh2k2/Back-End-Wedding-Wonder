using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class PhotographService
{
    public int ServiceId { get; set; }

    public virtual ICollection<PhotographPackage> PhotographPackages { get; } = new List<PhotographPackage>();

    public virtual ICollection<Photographer> Photographers { get; } = new List<Photographer>();

    public virtual Service Service { get; set; } = null!;
}

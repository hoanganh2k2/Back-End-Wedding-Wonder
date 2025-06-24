using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class Catering
{
    public int CateringId { get; set; }

    public string StyleName { get; set; } = null!;

    public string? PackageContent { get; set; }

    public int ServiceId { get; set; }

    public string? CateringImage { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<Menu> Menus { get; } = new List<Menu>();

    public virtual RestaurantService Service { get; set; } = null!;
}

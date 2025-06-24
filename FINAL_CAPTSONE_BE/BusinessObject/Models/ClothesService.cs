using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class ClothesService
{
    public int ServiceId { get; set; }

    public virtual ICollection<Outfit> Outfits { get; } = new List<Outfit>();

    public virtual Service Service { get; set; } = null!;
}

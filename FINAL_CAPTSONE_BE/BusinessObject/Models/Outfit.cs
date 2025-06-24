using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class Outfit
{
    public int OutfitId { get; set; }

    public string OutfitName { get; set; } = null!;

    public decimal? OutfitPrice { get; set; }

    public string? OutfitDescription { get; set; }

    public int ServiceId { get; set; }

    public int? Status { get; set; }

    public virtual ICollection<OutfitImage> OutfitImages { get; } = new List<OutfitImage>();

    public virtual ICollection<OutfitOutfitType> OutfitOutfitTypes { get; } = new List<OutfitOutfitType>();

    public virtual ClothesService Service { get; set; } = null!;
}

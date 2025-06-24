using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class OutfitOutfitType
{
    public int OutfitId { get; set; }

    public int OutfitTypeId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Outfit Outfit { get; set; } = null!;

    public virtual OutfitType OutfitType { get; set; } = null!;
}

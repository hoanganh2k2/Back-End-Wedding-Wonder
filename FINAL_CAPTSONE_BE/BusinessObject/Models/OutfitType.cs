using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class OutfitType
{
    public int OutfitTypeId { get; set; }

    public string TypeName { get; set; } = null!;

    public int? Status { get; set; }

    public virtual ICollection<OutfitOutfitType> OutfitOutfitTypes { get; } = new List<OutfitOutfitType>();
}

using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class OutfitImage
{
    public int ImageId { get; set; }

    public int OutfitId { get; set; }

    public string ImageText { get; set; } = null!;

    public virtual Outfit Outfit { get; set; } = null!;
}

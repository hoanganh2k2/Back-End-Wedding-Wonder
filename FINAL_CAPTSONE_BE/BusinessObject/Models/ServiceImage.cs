using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class ServiceImage
{
    public int ImageId { get; set; }

    public int ServiceId { get; set; }

    public string ImageText { get; set; } = null!;

    public virtual Service Service { get; set; } = null!;
}

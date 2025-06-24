using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class RequestImage
{
    public int ImageId { get; set; }

    public string ImageType { get; set; } = null!;

    public int RequestId { get; set; }

    public string ImageText { get; set; } = null!;

    public virtual RequestUpgradeSupplier Request { get; set; } = null!;
}

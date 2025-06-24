using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class InvitationImage
{
    public int ImageId { get; set; }

    public int PackageId { get; set; }

    public string ImageText { get; set; } = null!;

    public virtual InvitationPackage Package { get; set; } = null!;
}

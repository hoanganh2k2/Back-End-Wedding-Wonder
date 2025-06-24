using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class InvitationService
{
    public int ServiceId { get; set; }

    public virtual ICollection<InvitationPackage> InvitationPackages { get; } = new List<InvitationPackage>();

    public virtual Service Service { get; set; } = null!;
}

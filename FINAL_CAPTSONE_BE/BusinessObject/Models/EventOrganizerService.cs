using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class EventOrganizerService
{
    public int ServiceId { get; set; }

    public virtual ICollection<EventPackage> EventPackages { get; } = new List<EventPackage>();

    public virtual Service Service { get; set; } = null!;
}

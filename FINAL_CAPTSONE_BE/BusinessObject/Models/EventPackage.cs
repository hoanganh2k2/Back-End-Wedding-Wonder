using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class EventPackage
{
    public int PackageId { get; set; }

    public string PackageName { get; set; } = null!;

    public decimal? PackagePrice { get; set; }

    public string? PackageContent { get; set; }

    public int ServiceId { get; set; }

    public int EventType { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<EventConcept> EventConcepts { get; } = new List<EventConcept>();

    public virtual EventOrganizerService Service { get; set; } = null!;
}

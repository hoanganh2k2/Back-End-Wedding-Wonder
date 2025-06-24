using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class EventConcept
{
    public int ConceptId { get; set; }

    public string ConceptName { get; set; } = null!;

    public int PackageId { get; set; }

    public bool? Status { get; set; }

    public virtual EventPackage Package { get; set; } = null!;
}

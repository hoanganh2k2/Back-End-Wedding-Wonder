using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class BusySchedule
{
    public int ScheduleId { get; set; }

    public int ServiceId { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string? Content { get; set; }

    public virtual Service Service { get; set; } = null!;
}

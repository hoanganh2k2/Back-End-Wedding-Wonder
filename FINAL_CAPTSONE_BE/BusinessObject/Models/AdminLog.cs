using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class AdminLog
{
    public int LogId { get; set; }

    public int AdminId { get; set; }

    public string ActionType { get; set; } = null!;

    public string IpAddress { get; set; } = null!;

    public string DeviceType { get; set; } = null!;

    public string? LogDetail { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual User Admin { get; set; } = null!;
}

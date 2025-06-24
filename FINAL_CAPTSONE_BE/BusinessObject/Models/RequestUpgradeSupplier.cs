using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class RequestUpgradeSupplier
{
    public int RequestId { get; set; }

    public int? UserId { get; set; }

    public string? RequestContent { get; set; }

    public string? IdNumber { get; set; }

    public string? FullName { get; set; }

    public string? BusinessType { get; set; }

    public string? Status { get; set; }

    public string? RejectReason { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<RequestImage> RequestImages { get; } = new List<RequestImage>();

    public virtual User? User { get; set; }
}

using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class CustomerReview
{
    public int ReviewId { get; set; }

    public int UserId { get; set; }

    public int ServiceId { get; set; }

    public string? Content { get; set; }

    public int StarNumber { get; set; }

    public string? Reply { get; set; }

    public DateTime? CreatedAt { get; set; }

    public bool CanEdit { get; set; }

    public virtual Service Service { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}

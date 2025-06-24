using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class IncurredCost
{
    public int CostId { get; set; }

    public string? CostContent { get; set; }

    public decimal? Price { get; set; }
}

using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class ContractService
{
    public int ContractId { get; set; }

    public int ServiceId { get; set; }

    public string? Content { get; set; }

    public virtual Contract Contract { get; set; } = null!;

    public virtual Service Service { get; set; } = null!;
}

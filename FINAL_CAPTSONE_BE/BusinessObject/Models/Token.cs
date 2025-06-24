using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class Token
{
    public int UserId { get; set; }

    public string KeyName { get; set; } = null!;

    public string? KeyValue { get; set; }

    public DateTime? Expiration { get; set; }

    public virtual User User { get; set; } = null!;
}

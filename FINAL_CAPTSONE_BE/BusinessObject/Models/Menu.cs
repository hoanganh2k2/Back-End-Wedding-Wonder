using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class Menu
{
    public int MenuId { get; set; }

    public string MenuName { get; set; } = null!;

    public decimal? Price { get; set; }

    public string? MenuContent { get; set; }

    public int CateringId { get; set; }

    public int MenuType { get; set; }

    public bool? Status { get; set; }

    public virtual Catering Catering { get; set; } = null!;
}

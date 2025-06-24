using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class FavoriteService
{
    public int FavoriteId { get; set; }

    public int UserId { get; set; }

    public int ServiceId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Service Service { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}

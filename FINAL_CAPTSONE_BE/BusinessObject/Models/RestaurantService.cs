using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class RestaurantService
{
    public int ServiceId { get; set; }

    public int Capacity { get; set; }

    public virtual ICollection<Catering> Caterings { get; } = new List<Catering>();

    public virtual Service Service { get; set; } = null!;
}

using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class ServiceTopic
{
    public int ServiceId { get; set; }

    public int TopicId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Service Service { get; set; } = null!;

    public virtual Topic Topic { get; set; } = null!;
}

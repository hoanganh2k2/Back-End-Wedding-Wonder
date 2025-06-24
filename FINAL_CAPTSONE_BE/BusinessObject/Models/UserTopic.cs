using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class UserTopic
{
    public int UserId { get; set; }

    public int TopicId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Topic Topic { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}

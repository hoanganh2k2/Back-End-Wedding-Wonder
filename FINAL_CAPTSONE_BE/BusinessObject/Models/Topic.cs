using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class Topic
{
    public int TopicId { get; set; }

    public string Name { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<ServiceTopic> ServiceTopics { get; } = new List<ServiceTopic>();

    public virtual ICollection<Service> Services { get; } = new List<Service>();

    public virtual ICollection<UserTopic> UserTopics { get; } = new List<UserTopic>();
}

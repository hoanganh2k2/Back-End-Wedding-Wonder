using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class Blog
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public DateTime CreateDate { get; set; }

    public string? Image { get; set; }

    public virtual ICollection<Tag> Tags { get; } = new List<Tag>();
}

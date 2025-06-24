﻿using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class Tag
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Blog> Blogs { get; } = new List<Blog>();
}

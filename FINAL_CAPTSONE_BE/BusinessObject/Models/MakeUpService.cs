using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class MakeUpService
{
    public int ServiceId { get; set; }

    public virtual ICollection<MakeUpArtist> MakeUpArtists { get; } = new List<MakeUpArtist>();

    public virtual ICollection<MakeUpPackage> MakeUpPackages { get; } = new List<MakeUpPackage>();

    public virtual Service Service { get; set; } = null!;
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs
{
    public class MakeUpArtistDTO
    {
        public int ArtistId { get; set; }

        public string ArtistName { get; set; } = null!;

        public string? ArtistImage { get; set; }

        public int Experience { get; set; }

        public string? Services { get; set; }

        public string? Certifications { get; set; }

        public string? Awards { get; set; }

        public int ServiceId { get; set; }

        public bool? Status { get; set; }
    }
}

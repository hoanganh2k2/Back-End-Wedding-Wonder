using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs
{
    public class PhotographerDTO
    {
        public int PhotographerId { get; set; }

        public string PhotographerName { get; set; } = null!;

        public string? PhotographerImage { get; set; }

        public string? About { get; set; }

        public string? Artwork1 { get; set; }

        public string? Artwork2 { get; set; }

        public int ServiceId { get; set; }

        public bool? Status { get; set; }
    }
}

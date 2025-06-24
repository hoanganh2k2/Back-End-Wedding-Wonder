using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs
{
    public class FavoriteServiceDTO
    {
        public int FavoriteId { get; set; }            
        public DateTime CreatedAt { get; set; }         
        public int ServiceId { get; set; }
        public string? ServiceName { get; set; }
        public int SupplierId { get; set; }
        public int ServiceTypeId { get; set; }
        public int StarNumber { get; set; }
        public string? City { get; set; }
        public string? District { get; set; }
        public string? Ward { get; set; }
        public string? Address { get; set; }
        public string? Description { get; set; }
        public string? VisitWebsiteLink { get; set; }
        public List<string>? Images { get; set; }
    }
}

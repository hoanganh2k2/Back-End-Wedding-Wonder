using BusinessObject.DTOs;

namespace WeddingWonderAPI.Models
{
    public class ServiceDTO
    {
        public int ServiceId { get; set; }
        public int SupplierId { get; set; }
        public int ServiceTypeId { get; set; }
        public string ServiceName { get; set; } = null!;
        public string? Description { get; set; }
        public string? City { get; set; }
        public string? District { get; set; }
        public string? Ward { get; set; }
        public string? Address { get; set; }
        public string? AvatarImage { get; set; }
        public ICollection<string>? AllImage { get; set; }
        public decimal? StarNumber { get; set; }
        public string? VisitWebsiteLink { get; set; }
        public int? IsActive { get; set; }
        public bool IsVipSupplier { get; set; }  
        public List<int> serviceTopics { get; set; } = new List<int>();
        public int MatchingTopicsCount { get; set; }
        public List<BusyScheduleDTO>? BusySchedules { get; set; }
        public List<string> Images { get; set; } = new List<string>();
    }
}
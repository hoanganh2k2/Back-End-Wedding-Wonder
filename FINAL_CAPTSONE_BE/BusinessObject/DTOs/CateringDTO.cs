namespace BusinessObject.DTOs
{
    public class CateringDTO
    {
        public int CateringId { get; set; }
        public string StyleName { get; set; } = null!;
        public string? PackageContent { get; set; }
        public int ServiceId { get; set; }
        public string? CateringImage { get; set; }
        public bool? Status { get; set; }
    }
}

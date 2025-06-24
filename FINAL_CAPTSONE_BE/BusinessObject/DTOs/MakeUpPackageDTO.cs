namespace WeddingWonderAPI.Models.DTOs
{
    public class MakeUpPackageDTO
    {
        public int PackageId { get; set; }
        public string PackageName { get; set; } = null!;
        public decimal? PackagePrice { get; set; }
        public int ServiceId { get; set; }
        public string? PackageContent { get; set; }
        public int EventType { get; set; }
        public bool? Status { get; set; }
    }
}
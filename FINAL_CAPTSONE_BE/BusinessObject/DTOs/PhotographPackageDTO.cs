namespace WeddingWonderAPI.Models.DTOs
{
    public class PhotographPackageDTO
    {
        public int PackageId { get; set; }
        public string PackageName { get; set; } = null!;

        public int ServiceId { get; set; }
        public string? PackageContent { get; set; }
        public string? WorkFlow { get; set; }
        public int EventType { get; set; }
        public int PhotoType { get; set; }

        public string Location { get; set; } = null!;

        public string? ImageSamples { get; set; }
        public bool? Status { get; set; }
        public decimal? PackagePrice { get; set; }
    }
}
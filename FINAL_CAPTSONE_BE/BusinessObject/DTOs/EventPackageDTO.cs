namespace WeddingWonderAPI.Models.DTOs
{
    using Newtonsoft.Json;

    public class EventPackageDTO
    {
        [JsonProperty("packageId")]
        public int PackageId { get; set; }

        public required string PackageName { get; set; }
        public decimal PackagePrice { get; set; }
        public int ServiceId { get; set; }
        public string? PackageContent { get; set; }
        public int EventType { get; set; }

        public bool? Status { get; set; }
    }
}
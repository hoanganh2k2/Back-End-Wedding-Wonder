namespace WeddingWonderAPI.Models.DTO
{
    public class InvitationPackageDTO
    {
        public int PackageId { get; set; }
        public string PackageName { get; set; }
        public decimal? PackagePrice { get; set; }
        public int ServiceId { get; set; }

        public string? PackageDescribe { get; set; }

        public string? Envelope { get; set; }

        public string? Component { get; set; }

        public string? Size { get; set; }

        public bool? Status { get; set; }
    }
}
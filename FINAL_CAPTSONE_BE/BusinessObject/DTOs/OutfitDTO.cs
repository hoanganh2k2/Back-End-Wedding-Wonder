namespace BusinessObject.DTOs
{
    public class OutfitDTO
    {
        public int? OutfitId { get; set; }

        public string? OutfitName { get; set; } = null!;

        public decimal? OutfitPrice { get; set; }

        public string? OutfitDescription { get; set; }

        public string? AvatarImage { get; set; }

        public ICollection<string>? AllImage { get; set; }
        public ICollection<int>? OutfitTypes { get; set; }

        public int? ServiceId { get; set; }

        public int? Status { get; set; }
    }
}

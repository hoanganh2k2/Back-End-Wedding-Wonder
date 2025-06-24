namespace BusinessObject.DTOs
{
    public class CustomerReviewDTO
    {
        public int? ReviewId { get; set; }

        public int? UserId { get; set; }

        public int? ServiceId { get; set; }

        public string? Content { get; set; }

        public int? StarNumber { get; set; }

        public string? Reply { get; set; }

        public DateTime? CreatedAt { get; set; }

        public bool? CanEdit { get; set; }

        public string? FullName { get; set; } 
        public string? UserImage { get; set; } 
        public string? ServiceName { get; set; }
        public string? ServiceImage { get; set; } 
    }
}

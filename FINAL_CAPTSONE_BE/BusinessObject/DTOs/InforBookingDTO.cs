namespace BusinessObject.DTOs
{
    public class InforBookingDTO
    {
        public int InforId { get; set; }

        public string FullName { get; set; } = null!;

        public DateTime? DateOfBirth { get; set; }

        public string PhoneNumber { get; set; } = null!;

        public string City { get; set; } = null!;

        public string District { get; set; } = null!;

        public string Ward { get; set; } = null!;

        public string Address { get; set; } = null!;
    }
}

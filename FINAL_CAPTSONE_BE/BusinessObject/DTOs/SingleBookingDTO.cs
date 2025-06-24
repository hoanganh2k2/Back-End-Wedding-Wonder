namespace BusinessObject.DTOs
{
    public class SingleBookingDTO
    {
        public int? BookingId { get; set; }
        public DateTime DateOfUse { get; set; }
        public int ServiceId { get; set; }
        public int PackageId { get; set; }
        public string FullName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string City { get; set; } = null!;
        public string District { get; set; } = null!;
        public string Ward { get; set; } = null!;
        public string Address { get; set; } = null!;
        public int? NumberOfUses { get; set; }
    }
}

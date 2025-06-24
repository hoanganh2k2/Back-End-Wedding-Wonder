using BusinessObject.DTOs;

namespace BusinessObject.Requests
{
    public class SingleBookingToShow
    {
        public int? BookingId { get; set; }
        public string? UserImage { get; set; }
        public string? FullName { get; set; }
        public InforBookingDTO Infor { get; set; }
        public int ServiceId { get; set; }
        public int ServiceTypeId { get; set; }
        public int? PackageId { get; set; }
        public DateTime DateOfUse { get; set; }
        public DateTime CreatedAt { get; set; }
        public int BookingStatus { get; set; }
    }
}

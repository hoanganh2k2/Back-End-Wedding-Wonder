using BusinessObject.DTOs;

namespace BusinessObject.Requests
{
    public class BookingToShow
    {
        public int? BookingId { get; set; }
        public InforBookingDTO? Groom { get; set; }
        public InforBookingDTO? Bride { get; set; }
        public string? UserImage { get; set; }
        public string? FullName { get; set; }
        public InforBookingDTO? Infor { get; set; }
        public string? ServiceName { get; set; }
        public int ServiceId { get; set; }
        public int ServiceTypeId { get; set; }
        public int? PackageId { get; set; }
        public string? Note { get; set; }
        public decimal? TotalPrice { get; set; }
        public DateTime? DateOfUse { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int BookingStatus { get; set; }
        public int TypeBooking { get; set; }
    }
}

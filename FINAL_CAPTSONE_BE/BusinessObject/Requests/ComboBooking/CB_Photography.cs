namespace BusinessObject.Requests.ComboBooking
{
    public class CB_Photography
    {
        public int ServiceId { get; set; }
        public int PreWeddingPackageId { get; set; }
        public int WeddingPackageId { get; set; }
        public DateTime PreAppointment { get; set; }
        public string? Note { get; set; }
        public decimal? Price { get; set; }
        public int? Status { get; set; }
    }
}

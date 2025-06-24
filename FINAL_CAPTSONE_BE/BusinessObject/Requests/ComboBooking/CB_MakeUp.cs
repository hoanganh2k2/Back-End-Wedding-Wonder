namespace BusinessObject.Requests.ComboBooking
{
    public class CB_MakeUp
    {
        public int ServiceId { get; set; }
        public int PackageId { get; set; }
        public string? Note { get; set; }
        public decimal? Price { get; set; }
        public bool ServiceMode { get; set; }
        public int? Status { get; set; }
    }
}

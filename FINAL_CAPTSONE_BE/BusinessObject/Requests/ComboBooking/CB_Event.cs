namespace BusinessObject.Requests.ComboBooking
{
    public class CB_Event
    {
        public int ServiceId { get; set; }
        public int PackageId { get; set; }
        public int ConceptId { get; set; }
        public string? Note { get; set; }
        public decimal? Price { get; set; }
        public int? Status { get; set; }
    }
}

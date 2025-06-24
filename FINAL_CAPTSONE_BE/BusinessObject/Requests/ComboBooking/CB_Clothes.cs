namespace BusinessObject.Requests.ComboBooking
{
    public class CB_Clothes
    {
        public int ServiceId { get; set; }
        public DateTime FittingDay { get; set; }
        public string? Note { get; set; }
        public decimal? Price { get; set; }
        public int? Status { get; set; }
    }
}

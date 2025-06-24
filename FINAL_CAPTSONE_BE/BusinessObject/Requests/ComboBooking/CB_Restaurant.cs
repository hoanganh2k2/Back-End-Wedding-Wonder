namespace BusinessObject.Requests.ComboBooking
{
    public class CB_Restaurant
    {
        public int ServiceId { get; set; }
        public int CuisineTypeId { get; set; }
        public int MenuId { get; set; }
        public string? Note { get; set; }
        public int? NumberOfUses { get; set; }
        public decimal? Price { get; set; }
        public int? Status { get; set; }
    }
}

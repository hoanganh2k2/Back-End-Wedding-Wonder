namespace BusinessObject.Requests.ComboBooking
{
    public class SupplierUpdate
    {
        public int? DetailId { get; set; }
        public int? PrePackageId { get; set; }
        public int? PackageId { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal BasicPrice { get; set; }
        public string? Note { get; set; }
        public int? NumberOfUses { get; set; }
        public DateTime? PreAppointment { get; set; }
        public DateTime? Appointment { get; set; }
    }
}

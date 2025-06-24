namespace BusinessObject.Requests.ComboBooking
{
    public class CB_Invitation
    {
        public int ServiceId { get; set; }
        public int InvitationId { get; set; }
        public string? Note { get; set; }
        public decimal? Price { get; set; }
        public int? NumberOfUses { get; set; }
        public bool ServiceMode { get; set; }
        public int? Status { get; set; }
    }
}

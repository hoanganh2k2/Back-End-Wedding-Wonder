using BusinessObject.Requests.ComboBooking;

namespace BusinessObject.DTOs
{
    public class ComboBookingDTO
    {
        public int? BookingId { get; set; }
        public int? TypeCombo { get; set; }
        public InforBookingDTO Groom { get; set; }
        public InforBookingDTO Bride { get; set; }
        public DateTime ExpectedWeddingDate { get; set; }
        public CB_Clothes? ClothesOp1 { get; set; }
        public CB_Clothes? ClothesOp2 { get; set; }
        public CB_Event? EventOp1 { get; set; }
        public CB_Event? EventOp2 { get; set; }
        public CB_Invitation? InvitationOp1 { get; set; }
        public CB_Invitation? InvitationOp2 { get; set; }
        public CB_MakeUp? MakeUpOp1 { get; set; }
        public CB_MakeUp? MakeUpOp2 { get; set; }
        public CB_Photography? PhotoOp1 { get; set; }
        public CB_Photography? PhotoOp2 { get; set; }
        public CB_Restaurant? RestaurantOp1 { get; set; }
        public CB_Restaurant? RestaurantOp2 { get; set; }
        public int? VoucherId { get; set; }
    }
}

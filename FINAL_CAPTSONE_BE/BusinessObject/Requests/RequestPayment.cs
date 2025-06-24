namespace BusinessObject.Requests
{
    public class RequestPayment
    {
        public int? RequestPaymentid { get; set; }
        public DateTime Deadline { get; set; }
        public decimal RequiredAmount { get; set; }
        public string Content { get; set; }
    }
}

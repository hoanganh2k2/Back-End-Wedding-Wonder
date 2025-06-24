using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class BookingTransactionHistory
{
    public int TransactionId { get; set; }

    public DateTime TransactionDate { get; set; }

    public string? TransactionType { get; set; }

    public decimal? NumberPrice { get; set; }

    public int? BookingId { get; set; }

    public virtual ComboBooking? Booking { get; set; }
}

using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class VoucherAdmin
{
    public int VoucherId { get; set; }

    public int TypeOfCombo { get; set; }

    public int TypeOfDiscount { get; set; }

    public decimal? Percent { get; set; }

    public decimal? Amount { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<ComboBooking> ComboBookings { get; } = new List<ComboBooking>();
}

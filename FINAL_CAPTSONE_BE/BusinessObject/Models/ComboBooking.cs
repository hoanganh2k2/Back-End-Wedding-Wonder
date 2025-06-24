using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class ComboBooking
{
    public int BookingId { get; set; }

    public DateTime ExpectedWeddingDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public int UserId { get; set; }

    public int? VoucherId { get; set; }

    public decimal BasicPrice { get; set; }

    public decimal TotalPrice { get; set; }

    public decimal? Budget { get; set; }

    public DateTime? DepositDate { get; set; }

    public decimal? MinRequiredDeposit { get; set; }

    public int? ContractId { get; set; }

    public int TypeCombo { get; set; }

    public int InforBrideId { get; set; }

    public int InforGroomId { get; set; }

    public int BookingStatus { get; set; }

    public virtual ICollection<BookingServiceDetail> BookingServiceDetails { get; } = new List<BookingServiceDetail>();

    public virtual ICollection<BookingTransactionHistory> BookingTransactionHistories { get; } = new List<BookingTransactionHistory>();

    public virtual Contract? Contract { get; set; }

    public virtual ICollection<Contractbooking> Contractbookings { get; } = new List<Contractbooking>();

    public virtual InforBooking InforBride { get; set; } = null!;

    public virtual InforBooking InforGroom { get; set; } = null!;

    public virtual User User { get; set; } = null!;

    public virtual VoucherAdmin? Voucher { get; set; }
}

using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class Transaction
{
    public int TransactionId { get; set; }

    public int? UserId { get; set; }

    public double? Amount { get; set; }

    public string? Reason { get; set; }

    public DateTime? RequestDate { get; set; }

    public string? Status { get; set; }

    public DateTime? ProcessedDate { get; set; }

    public string TransactionType { get; set; } = null!;

    public string? CardNumber { get; set; }

    public string? CardHolderName { get; set; }

    public string? BankName { get; set; }
}

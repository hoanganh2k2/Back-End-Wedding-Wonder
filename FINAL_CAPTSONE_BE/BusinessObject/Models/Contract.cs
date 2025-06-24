using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class Contract
{
    public int ContractId { get; set; }

    public int? UserId { get; set; }

    public string Title { get; set; } = null!;

    public string PdfFilePath { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public string? FrontIdCardPath { get; set; }

    public string? BackIdCardPath { get; set; }

    public bool? IsConfirmed { get; set; }

    public string? Otp { get; set; }

    public DateTime? SignedDate { get; set; }

    public virtual ICollection<ComboBooking> ComboBookings { get; } = new List<ComboBooking>();

    public virtual ICollection<ContractService> ContractServices { get; } = new List<ContractService>();

    public virtual User? User { get; set; }
}

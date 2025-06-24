using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs
{
    public class TransactionDTO
    {
        public int TransactionId { get; set; }

        public int? UserId { get; set; }

        public double? Amount { get; set; }

        public string? Reason { get; set; }

        public DateTime? RequestDate { get; set; }

        public string? Status { get; set; } // 1 -pending , 2 -acp , 3 reject

        public DateTime? ProcessedDate { get; set; }

        public string TransactionType { get; set; } = null!;

        public string? CardNumber { get; set; }

        public string? CardHolderName { get; set; }

        public string? BankName { get; set; }
    }
} 
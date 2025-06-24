using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs
{
    public class RequestUpgradeSupplierDTO
    {
        public int RequestId { get; set; }
        public int? UserId { get; set; }
        public string? RequestContent { get; set; }
        public string? BusinessType { get; set; }
        public string? Status { get; set; }
        public string? RejectReason { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? IdNumber { get; set; }
        public string? FullName { get; set; }
        public List<RequestImageDTO>? RequestImages { get; set; }
    }
}

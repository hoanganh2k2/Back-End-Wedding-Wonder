using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs
{
    public class VoucherAdminDTO
    {
        public int VoucherId { get; set; }
        public int TypeOfCombo { get; set; }
        public int TypeOfDiscount { get; set; }
        public decimal? Percent { get; set; }
        public decimal? Amount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}

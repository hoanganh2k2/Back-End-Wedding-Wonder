using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs
{
    public class CreateRequestDTO
    {
        public RequestUpgradeSupplierDTO RequestDto { get; set; }
        public List<RequestImageDTO> ImageDtos { get; set; }
    }
}

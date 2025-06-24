using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs
{
    public class RequestImageDTO
    {
        public int ImageId { get; set; }
        public int RequestId { get; set; }
        public string ImageText { get; set; }
        public string ImageType { get; set; }
    }
}

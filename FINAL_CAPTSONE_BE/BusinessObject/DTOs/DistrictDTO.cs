using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs
{
    public class DistrictDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<WardDTO> Wards { get; set; }
    }
}

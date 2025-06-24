using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs
{
    public class AdminLogDTO
    {
        public int LogId { get; set; }
        public int AdminId { get; set; }

        public string IpAddress { get; set; }

        public string DeviceType { get; set; }
        public string ActionType { get; set; }
        public string LogDetail { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
         
        public string AdminName { get; set; }
    }
}

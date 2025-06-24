using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs
{
    public class MessageRequest
    {
        public int ReceiverId { get; set; }
        public string MessageContent { get; set; }
    }
}

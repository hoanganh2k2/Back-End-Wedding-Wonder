using BusinessObject.Contracts.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Contracts.IntergrationEvents
{
    public class EmailNotificationEvent : INotificationEvent
    {
        public Guid Id { get; set; }
        public string ReceiverEmail { get; set; }
        public string ReceiverName { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
    }
}

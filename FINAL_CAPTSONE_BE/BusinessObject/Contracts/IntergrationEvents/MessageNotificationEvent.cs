using BusinessObject.Contracts.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Contracts.IntergrationEvents
{
    public class MessageNotificationEvent : INotificationEvent
    {
        public Guid Id { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string MessageContent { get; set; }
        public string NotificationType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public Guid TransactionId { get; set; }
    }
}

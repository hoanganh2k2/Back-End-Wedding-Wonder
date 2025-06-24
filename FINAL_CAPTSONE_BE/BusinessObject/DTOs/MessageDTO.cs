using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs
{
    public class MessageDTO
    {
        public int MessageId { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string? Content { get; set; }
        public DateTime SendDate { get; set; }
        public int ConversationId { get; set; }
        public string? AttachmentUrl { get; set; }
        public string? Type { get; set; }
        public int? ReplyToMessageId { get; set; }
        public string? RepliedMessageContent { get; set; }
    }
}

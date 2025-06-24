using System;
using System.Collections.Generic;

namespace BusinessObject.Models;

public partial class Message
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

    public virtual User Receiver { get; set; } = null!;

    public virtual User Sender { get; set; } = null!;
}

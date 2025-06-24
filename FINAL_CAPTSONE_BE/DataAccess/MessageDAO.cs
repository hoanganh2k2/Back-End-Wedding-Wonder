using BusinessObject.DTOs;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class MessageDAO
    {
        private readonly WeddingWonderDbContext context;

        public MessageDAO(WeddingWonderDbContext context)
        {
            this.context = context;
        } 

        public async Task<List<Message>> GetMessages()
        {
            try
            {
                return await context.Messages
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<int>> GetConversationIdsForUser(int userId)
        {
            try
            {
                return await context.Messages
                    .Where(m => m.SenderId == userId || m.ReceiverId == userId)
                    .Select(m => m.ConversationId)
                    .Distinct()  
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<List<UserDTO>> GetReceiversForUser(int userId)
        {
            try
            {
                var messages = await context.Messages
                    .Where(m => m.SenderId == userId || m.ReceiverId == userId)
                    .Select(m => new
                    {
                        OtherUser = m.SenderId == userId ? m.Receiver : m.Sender,
                        ConversationId = m.ConversationId,
                        IsOnline = m.SenderId == userId ? m.Receiver.IsOnline : m.Sender.IsOnline
                    })
                    .ToListAsync(); 

                var groupedMessages = messages
                    .GroupBy(r => new { r.OtherUser.UserId, r.ConversationId })
                    .Select(g => new UserDTO
                    {
                        UserId = g.Key.UserId,
                        FullName = g.First().OtherUser.FullName,
                        UserName = g.First().OtherUser.UserName,
                        Email = g.First().OtherUser.Email,
                        UserImage = g.First().OtherUser.UserImage,
                        IsOnline = g.First().IsOnline,
                        ConversationId = g.Key.ConversationId
                    })
                    .ToList();  

                return groupedMessages;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        public async Task<List<MessageDTO>> GetMessagesByConversationId(int conversationId)
        {
            try
            {
                var messages = await context.Messages
                    .Where(m => m.ConversationId == conversationId)
                    .OrderBy(m => m.SendDate)
                    .ToListAsync();

                var replyMessageIds = messages
                    .Where(m => m.ReplyToMessageId.HasValue)
                    .Select(m => m.ReplyToMessageId.Value)
                    .ToList();

                var repliedMessages = await context.Messages
                    .Where(m => replyMessageIds.Contains(m.MessageId))
                    .ToDictionaryAsync(m => m.MessageId, m => m.Content);

                return messages.Select(m => new MessageDTO
                {
                    MessageId = m.MessageId,
                    SenderId = m.SenderId,
                    ReceiverId = m.ReceiverId,
                    Content = m.Content,
                    SendDate = m.SendDate,
                    ConversationId = m.ConversationId,
                    AttachmentUrl = m.AttachmentUrl,
                    Type = m.Type,
                    ReplyToMessageId = m.ReplyToMessageId,
                    RepliedMessageContent = m.ReplyToMessageId.HasValue ? repliedMessages[m.ReplyToMessageId.Value] : null
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        public async Task<List<Message>> GetConversation(int senderId, int receiverId)
        {
            try
            {
                return await context.Messages
                    .Where(m => (m.SenderId == senderId && m.ReceiverId == receiverId) ||
                                (m.SenderId == receiverId && m.ReceiverId == senderId))
                    .OrderBy(m => m.SendDate)   
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<int> EnsureConversationExists(int senderId, int receiverId)
        {
            var conversationId = await GetConversationId(senderId, receiverId);

            if (conversationId == null)
            {
                conversationId = GenerateNewConversationId(senderId, receiverId);
            }

            return conversationId.Value;
        }

        public async Task<int?> GetConversationId(int senderId, int receiverId)
        {
            try
            {
                var conversation = await context.Messages
                    .Where(m => (m.SenderId == senderId && m.ReceiverId == receiverId) ||
                                (m.SenderId == receiverId && m.ReceiverId == senderId))
                    .Select(m => m.ConversationId)
                    .FirstOrDefaultAsync();

                return conversation;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> CreateMessage(Message message)
        {
            try
            { 
                var senderExists = await context.Users.AnyAsync(u => u.UserId == message.SenderId);
                var receiverExists = await context.Users.AnyAsync(u => u.UserId == message.ReceiverId);

                if (!senderExists || !receiverExists)
                {
                    throw new Exception("Sender or Receiver does not exist.");
                }
                 
                var conversationId = await GetConversationId(message.SenderId, message.ReceiverId);

                if (conversationId == null)
                { 
                    message.ConversationId = GenerateNewConversationId(message.SenderId, message.ReceiverId);
                }
                else
                { 
                    message.ConversationId = conversationId.Value;
                }
                if (message.Type == "location" && !string.IsNullOrEmpty(message.Content))
                {
                    var coordinates = message.Content.Split(',');

                    if (coordinates.Length == 2 &&
                        double.TryParse(coordinates[0], out _) &&
                        double.TryParse(coordinates[1], out _))
                    {
                        message.Type = "location";
                    }
                    else
                    {
                        throw new Exception("Invalid location format.");
                    }
                }
                else if (!string.IsNullOrEmpty(message.AttachmentUrl))
                {
                    var extension = Path.GetExtension(message.AttachmentUrl).ToLower();
                    if (extension == ".png" || extension == ".jpg" || extension == ".jpeg")
                    {
                        message.Type = "image";
                    }
                    else if (extension == ".pdf" || extension == ".doc" || extension == ".docx")
                    {
                        message.Type = "document";
                    }
                    else
                    {
                        message.Type = "unknown";
                    }
                }
                else
                {
                    message.Type = "text";
                }

                await context.Messages.AddAsync(message);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<int> CreateConversation(int senderId, int receiverId)
        {
            int conversationId = GenerateNewConversationId(senderId, receiverId);

            Message initialMessage = new Message
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Content = "Bắt đầu cuộc trò chuyện",
                SendDate = DateTime.UtcNow,
                ConversationId = conversationId,
                Type = "text"
            };

            await context.Messages.AddAsync(initialMessage);
            await context.SaveChangesAsync();

            return conversationId;
        }

        private int GenerateNewConversationId(int senderId, int receiverId)
        {
            return Math.Min(senderId, receiverId) * 100000 + Math.Max(senderId, receiverId);
        }
         
        public async Task<Message> GetMessageById(int messageId)
        {
            try
            {
                return await context.Messages
                    .FirstOrDefaultAsync(m => m.MessageId == messageId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
         
        public async Task UpdateMessage(int id, Message message)
        {
            try
            {
                context.Messages.Update(message);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
         
        public async Task DeleteMessage(int messageId)
        {
            try
            {
                Message? messageToDelete = await context.Messages.FindAsync(messageId);
                if (messageToDelete != null)
                {
                    context.Messages.Remove(messageToDelete);
                    await context.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("Message not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<Message>> SearchMessages(int userId, int conversationId, string key)
        {
            try
            {
                return await context.Messages
                    .Where(m => (m.SenderId == userId || m.ReceiverId == userId)
                                && m.ConversationId == conversationId
                                && m.Content.Contains(key))
                    .OrderBy(m => m.SendDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}

using BusinessObject.Contracts.IntergrationEvents;
using BusinessObject.DTOs;
using BusinessObject.Models;
using MassTransit;
using Repository.IRepositories;

namespace Services
{
    public class MessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly NotificationService _notificationService;
        private readonly IPublishEndpoint _publishEndpoint;

        public MessageService(
            IMessageRepository messageRepository,
            IUnitOfWork unitOfWork,
            NotificationService notificationService,
            IPublishEndpoint publishEndpoint)
        {
            _messageRepository = messageRepository;
            _unitOfWork = unitOfWork;
            _notificationService = notificationService;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<List<int>> GetConversationsForUserAsync(int userId)
        {
            return await _messageRepository.GetConversationIdsForUser(userId);
        }

        public async Task<List<UserDTO>> GetReceiversForUserAsync(int userId)
        {
            return await _messageRepository.GetReceiversForUserAsync(userId);
        }

        public async Task<int> StartConversationAsync(int senderId, int receiverId)
        {
            var conversationId = await _messageRepository.GetConversationId(senderId, receiverId);

            if (conversationId == 0)
            {
                conversationId = await _messageRepository.CreateConversation(senderId, receiverId);
            }

            return conversationId.Value;
        }

        public async Task<List<MessageDTO>> GetAllMessagesAsync()
        {
            try
            {
                List<Message> messages = await _messageRepository.GetsAsync();
                return messages.Select(m => new MessageDTO
                {
                    MessageId = m.MessageId,
                    SenderId = m.SenderId,
                    ReceiverId = m.ReceiverId,
                    Content = m.Content,
                    SendDate = m.SendDate,
                    ConversationId = (int)m.ConversationId
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<MessageDTO?> GetMessageByIdAsync(int id)
        {
            try
            {
                Message message = await _messageRepository.GetAsyncById(id);
                if (message == null) return null;

                return new MessageDTO
                {
                    MessageId = message.MessageId,
                    SenderId = message.SenderId,
                    ReceiverId = message.ReceiverId,
                    Content = message.Content,
                    SendDate = message.SendDate,
                    ConversationId = (int)message.ConversationId
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<MessageDTO>> GetMessagesByConversationIdAsync(int conversationId)
        {
            List<MessageDTO> messages = await _messageRepository.GetMessagesByConversationId(conversationId);
            return messages.Select(m => new MessageDTO
            {
                MessageId = m.MessageId,
                SenderId = m.SenderId,
                ReceiverId = m.ReceiverId,
                Content = m.Content,
                SendDate = m.SendDate,
                ConversationId = (int)m.ConversationId,
                AttachmentUrl = m.AttachmentUrl,
                Type = m.Type,
                ReplyToMessageId = m.ReplyToMessageId,
                RepliedMessageContent = m.RepliedMessageContent
            }).ToList();
        }

        public async Task<bool> CreateMessageAsync(MessageDTO messageDto)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                int senderExists = messageDto.SenderId;
                var receiverExists = messageDto.ReceiverId;

                if (senderExists == null || receiverExists == null)
                {
                    throw new Exception("Sender or Receiver does not exist.");
                }

                Message message = new()
                {
                    SenderId = messageDto.SenderId,
                    ReceiverId = messageDto.ReceiverId,
                    Content = messageDto.Content,
                    SendDate = messageDto.SendDate,
                    AttachmentUrl = messageDto.AttachmentUrl,
                    Type = messageDto.Type,
                    ReplyToMessageId = messageDto.ReplyToMessageId
                };

                var conversationId = await _messageRepository.GetConversationId(message.SenderId, message.ReceiverId);

                if (conversationId == null)
                {
                    message.ConversationId = GenerateNewConversationId(message.SenderId, message.ReceiverId);
                }
                else
                {
                    message.ConversationId = conversationId.Value;
                }

                await _messageRepository.CreateAsync(message);

                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
                var notificationEvent = new MessageNotificationEvent
                {
                    Id = Guid.NewGuid(),
                    TimeStamp = DateTimeOffset.Now,
                    SenderId = messageDto.SenderId,
                    ReceiverId = messageDto.ReceiverId,
                    MessageContent = messageDto.Content,
                    NotificationType = "Message",
                    Name = "New Message Notification",
                    Description = "You have a new message."
                };

                await _publishEndpoint.Publish(notificationEvent);
                return true;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> UpdateMessageAsync(int id, MessageDTO messageDto)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                Message existingMessage = await _messageRepository.GetAsyncById(id);
                if (existingMessage == null) return false;

                existingMessage.Content = messageDto.Content;
                existingMessage.SendDate = messageDto.SendDate;

                await _messageRepository.UpdateAsync(id, existingMessage);
                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();

                return true;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<bool> DeleteMessageAsync(int id)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                await _messageRepository.DeleteAsync(id);
                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
                return true;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception(ex.Message, ex);
            }
        }
        private int GenerateNewConversationId(int senderId, int receiverId)
        { 
            return Math.Min(senderId, receiverId) * 100000 + Math.Max(senderId, receiverId);
        }

        public async Task<List<MessageDTO>> SearchMessagesAsync(int userId, int conversationId, string key)
        {
            List<Message> messages = await _messageRepository.SearchMessages(userId, conversationId, key);

            return messages.Select(m => new MessageDTO
            {
                MessageId = m.MessageId,
                SenderId = m.SenderId,
                ReceiverId = m.ReceiverId,
                Content = m.Content,
                SendDate = m.SendDate,
                ConversationId = m.ConversationId,
                Type = m.Type,
                AttachmentUrl = m.AttachmentUrl
            }).ToList();
        }
    }
}

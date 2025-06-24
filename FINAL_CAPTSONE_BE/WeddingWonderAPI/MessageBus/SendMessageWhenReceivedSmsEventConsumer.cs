using BusinessObject.Contracts.IntergrationEvents;
using BusinessObject.Models;
using Fleck;
using MassTransit;
using Newtonsoft.Json;
using Repository.IRepositories;
using System.Collections.Concurrent;

namespace WeddingWonderAPI.MessageBus
{
    public class SendMessageWhenReceivedSmsEventConsumer : IConsumer<MessageNotificationEvent>
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ConcurrentDictionary<int, IWebSocketConnection> _wsConnections;

        public SendMessageWhenReceivedSmsEventConsumer(INotificationRepository notificationRepository, ConcurrentDictionary<int, IWebSocketConnection> wsConnections, IUnitOfWork unitOfWork)
        {
            _notificationRepository = notificationRepository;
            _wsConnections = wsConnections;
            _unitOfWork = unitOfWork;
        }

        public async Task Consume(ConsumeContext<MessageNotificationEvent> context)
        {
            await _unitOfWork.BeginTransactionAsync();
            var message = context.Message;
            //await Task.Delay(10000);
            try
            {
                var notification = new Notification
                {
                    ReceiverId = message.ReceiverId,
                    Content = message.MessageContent,
                    CreatedAt = DateTime.UtcNow,
                    IsRead = false
                };
                await _notificationRepository.AddNotificationAsync(notification);
                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();
                if (_wsConnections.TryGetValue(message.ReceiverId, out var receiverConnection))
                {
                    var response = new
                    {
                        type = message.NotificationType,
                        name = message.Name,
                        description = message.Description,
                        receiverId = message.ReceiverId,
                        content = message.MessageContent
                    };
                    _ = receiverConnection.Send(JsonConvert.SerializeObject(response));
                    Console.WriteLine($"Notification pushed to WebSocket for ReceiverId: {message.ReceiverId}");
                }
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw new Exception($"Error processing notificatio: {ex.Message}", ex);
            }
        }
    }
}

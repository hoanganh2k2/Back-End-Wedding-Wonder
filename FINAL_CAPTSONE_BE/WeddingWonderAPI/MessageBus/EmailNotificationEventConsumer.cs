using BusinessObject.Contracts.IntergrationEvents;
using MassTransit;
using MimeKit;
using Services.Email;

namespace WeddingWonderAPI.MessageBus
{
    namespace WeddingWonderAPI.MessageBus.Consumers
    {
        public class EmailNotificationEventConsumer : IConsumer<EmailNotificationEvent>
        {
            private readonly IEmailService _emailService;

            public EmailNotificationEventConsumer(IEmailService emailService)
            {
                _emailService = emailService;
            }

            public async Task Consume(ConsumeContext<EmailNotificationEvent> context)
            {
                var emailEvent = context.Message;

                try
                {
                    var message = new Message(
                        new List<MailboxAddress>
                        {
                        new MailboxAddress(emailEvent.ReceiverName, emailEvent.ReceiverEmail)
                        },
                        emailEvent.Subject,
                        emailEvent.Body
                    );

                    await _emailService.SendEmail(message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error sending email: {ex.Message}");
                    throw;
                }
            }
        }
    }
}

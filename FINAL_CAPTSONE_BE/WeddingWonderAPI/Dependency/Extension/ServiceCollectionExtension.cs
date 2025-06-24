using MassTransit;
using WeddingWonderAPI.Dependency.Option;
using WeddingWonderAPI.MessageBus;
using WeddingWonderAPI.MessageBus.WeddingWonderAPI.MessageBus.Consumers;

namespace WeddingWonderAPI.Dependency.Extension
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddConfigureMasstransitRabbitMQ(this IServiceCollection services, IConfiguration configuration)
        {
            var masstransitConfiguration = new MasstransitConfiguration();
            configuration.GetSection("MasstransitConfiguration").Bind(masstransitConfiguration);

            services.AddMassTransit(mt =>
            {
                mt.AddConsumer<SendMessageWhenReceivedSmsEventConsumer>();
                mt.AddConsumer<EmailNotificationEventConsumer>();

                mt.UsingRabbitMq((context, bus) =>
                {
                    bus.Host(masstransitConfiguration.Host, masstransitConfiguration.VHost, h =>
                    {
                        h.Username(masstransitConfiguration.UserName);
                        h.Password(masstransitConfiguration.Password);
                    });

                    bus.ReceiveEndpoint(masstransitConfiguration.NotificationQueueName, e =>
                    {
                        e.ConfigureConsumer<SendMessageWhenReceivedSmsEventConsumer>(context);
                        e.ConfigureConsumer<EmailNotificationEventConsumer>(context);
                    });
                });
            });

            return services;
        }
    }
}

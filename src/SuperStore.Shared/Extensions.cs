using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using SuperStore.Shared.Connections;
using SuperStore.Shared.Publishers;

namespace SuperStore.Shared
{
    public static class Extensions
    {
        public static IServiceCollection AddMessaging(this IServiceCollection services)
        {
            var connectionFactory = new ConnectionFactory()
            {
                HostName = "LocalHost"
            };
            var connection = connectionFactory.CreateConnection();
            
            services.AddSingleton(connection);
            services.AddSingleton<ChannelAccessor>();
            services.AddSingleton<IChannelFactory, ChannelFactory>();
            services.AddSingleton<IMessagePublisher, MessagePublisher>();
            return services;
        }

    }
}
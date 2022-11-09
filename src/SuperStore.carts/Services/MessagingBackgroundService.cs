using SuperStore.carts.Messages;
using SuperStore.Shared;

namespace SuperStore.carts.Services;

internal sealed class MessagingBackgroundService : BackgroundService
{
    private readonly IMessageSubscriber _subscriber;
    private readonly IChannelFactory _channelFactory;
    private readonly ILogger<MessagingBackgroundService> _logger;

    public MessagingBackgroundService(
        IChannelFactory channelFactory,
        ILogger<MessagingBackgroundService> logger,
        IMessageSubscriber subscriber)
    {
        _channelFactory = channelFactory;
        _logger = logger;
        _subscriber = subscriber;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var channel = _channelFactory.Create();
        channel.ExchangeDeclare("Funds","topic",durable:false,autoDelete:false,null);
        _subscriber.Subscribe<FundsMessage>(
            "carts-service-funds-message",
            "FundsMessage",
            "Funds",
            (message, ea) =>
            {
                _logger.LogInformation($"CustomerId:{message.CustomerId} | CurrentFunds:{message.CurrentFunds} | " +
                                       $"RoutingKey:{ea.RoutingKey}");
                return Task.CompletedTask;
            });

    }


}
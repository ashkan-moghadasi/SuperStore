using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace SuperStore.Shared.Publishers;

internal class MessagePublisher : IMessagePublisher
{
    private readonly IModel _channel;

    public MessagePublisher(IChannelFactory channelFactory)
    {
        _channel = channelFactory.Create();
    }

    public Task PublishAsync<TMessage>(string exchange, string routingKey, TMessage message) where TMessage : class, IMessage
    {
        var json = JsonSerializer.Serialize(message);
        var body=Encoding.UTF8.GetBytes(json);
        var properties = _channel.CreateBasicProperties();
        _channel.ExchangeDeclare(exchange,"topic",false,false);
        _channel.BasicPublish(exchange,routingKey,properties,body);
        return Task.CompletedTask;
    }
}
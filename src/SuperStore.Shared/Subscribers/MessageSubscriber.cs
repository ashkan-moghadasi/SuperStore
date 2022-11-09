using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text.Json;
namespace SuperStore.Shared.Subscribers;

internal sealed class MessageSubscriber : IMessageSubscriber
{
    private readonly IModel _channel;

    public MessageSubscriber(IChannelFactory factory)
    =>_channel=factory.Create();

    public IMessageSubscriber Subscribe<TMessage>(string queueName, string routingKey, string exchangeName, Func<TMessage, BasicDeliverEventArgs, Task> handle) where TMessage : class, IMessage
    {
        _channel.QueueDeclare(queueName, durable: true, exclusive: false, autoDelete: false);
        _channel.QueueBind(queueName,exchangeName,routingKey);

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received +=  async(model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = JsonSerializer.Deserialize<TMessage>(Encoding.UTF8.GetString(body));
            await handle.Invoke(message, ea);
        };
        _channel.BasicConsume(queueName, autoAck: true, consumer);
        return this;
    }
}
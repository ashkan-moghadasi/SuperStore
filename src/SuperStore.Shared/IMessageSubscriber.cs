using RabbitMQ.Client.Events;

namespace SuperStore.Shared;

public interface IMessageSubscriber
{
    IMessageSubscriber Subscribe<TMessage>(string queueName,string routingKey,string exchangeName,
        Func<TMessage,BasicDeliverEventArgs,Task> Handle) 
        where TMessage : class, IMessage;
}
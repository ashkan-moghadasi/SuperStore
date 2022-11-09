using RabbitMQ.Client;

namespace SuperStore.Shared;

public interface IChannelFactory
{
    public IModel Create();
}
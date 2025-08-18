using RabbitMQ.Client;

namespace PaymentProcessor.Api.Infrastructure.MessageBroker;

public interface IRabbitMQConnection
{
    IConnection Connection { get; }
}

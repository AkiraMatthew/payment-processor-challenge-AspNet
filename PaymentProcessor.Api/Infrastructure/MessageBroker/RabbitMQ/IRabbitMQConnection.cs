using RabbitMQ.Client;

namespace PaymentProcessor.Api.Infrastructure.MessageBroker.RabbitMQ;

public interface IRabbitMQConnection
{
    IConnection Connection { get; }
    IDisposable Disposable { get; }
}

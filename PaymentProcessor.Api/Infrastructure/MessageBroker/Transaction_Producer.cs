using RabbitMQ.Client;
using System.Text;

namespace PaymentProcessor.Api.Infrastructure.MessageBroker;

public class Transaction_Producer : IRabbitMQConnection
{
    private readonly string _connectionString;
    private readonly IConnection _connection;
    private readonly IDisposable _disposable;
    public IConnection Connection => _connection;
    public IDisposable Disposable => _disposable;

    public Transaction_Producer(string connectionString)
    {
        _connectionString = connectionString;
    }

    private async Task<IConnection> InitializeConnection()
    {
        ConnectionFactory factory = new()
        {
            HostName = "localhost",
            Uri = new Uri(_connectionString)
        };

        return await factory.CreateConnectionAsync();
    }

    public async Task Producer()
    {
        using var connection = await InitializeConnection();
        using var channel = await connection.CreateChannelAsync();

        await channel.ExchangeDeclareAsync(
            exchange: "transaction",
            durable: true,
            type: ExchangeType.Fanout,
            autoDelete: false,
            arguments: null);

        const string message = "Hello World!";
        var body = Encoding.UTF8.GetBytes(message);

        await channel.BasicPublishAsync(exchange: string.Empty, routingKey: "hello", body: body);
        Console.WriteLine($" [x] Sent {message}");

        Console.WriteLine(" Press [enter] to exit.");
        Console.ReadLine();
    }
}

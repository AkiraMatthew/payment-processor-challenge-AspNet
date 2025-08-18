using RabbitMQ.Client;
using System.Text;

namespace PaymentProcessor.Api.Infrastructure.MessageBroker;

public class Transaction_Producer : IRabbitMQConnection, IDisposable
{
    private readonly string _connectionString;
    private readonly IConnection _connection;
    public IConnection Connection => _connection;

    public Transaction_Producer(string connectionString)
    {
        _connectionString = connectionString;
        InitializeConnection();
    }

    private async Task InitializeConnection()
    {
        ConnectionFactory factory = new()
        {
            HostName = "localhost",
            Uri = new Uri(_connectionString)
        };
        await factory.CreateConnectionAsync();
    }

    public async Task Producer()
    {
        ConnectionFactory factory = new()
        {
            HostName = "localhost",
            Uri = new Uri(_connectionString)
        };
        using var connection = await factory.CreateConnectionAsync();
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

    public void Dispose() => _connection?.Dispose();
}

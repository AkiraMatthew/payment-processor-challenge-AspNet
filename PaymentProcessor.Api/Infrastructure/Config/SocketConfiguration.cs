namespace PaymentProcessor.Api.Infrastructure.Config;

public class SocketConfiguration
{
    public const int MaxDegreeOfParallels = 20;

    public static Func<HttpMessageHandler> GetSocketHandler() => () => new SocketsHttpHandler
    {
        MaxConnectionsPerServer = int.MaxValue,
    };
}

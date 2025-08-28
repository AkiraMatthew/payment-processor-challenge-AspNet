namespace PaymentProcessor.Api.Infrastructure.Config;

public class HttpClientConfig
{
    public const int MaxDegreeOfParallels = 20;

    public static Func<HttpMessageHandler> GetSocketHandler() => () => new SocketsHttpHandler
    {
        MaxConnectionsPerServer = int.MaxValue,
    };
}

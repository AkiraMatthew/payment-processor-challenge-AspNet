namespace PaymentProcessor.Api.Features.PaymentProcessor;

public static class PaymentsEndpoints
{
    public static void MapPaymentsApi(this WebApplication app)
    {
        app.MapPost("/payments", () => "HelloWorld");
        app.MapPost("/payments-summary", () => "HelloWorld");
    }
}
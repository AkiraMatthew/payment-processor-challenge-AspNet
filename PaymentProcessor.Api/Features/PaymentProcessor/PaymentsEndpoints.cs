using PaymentProcessor.Api.Domain.DTOs.POST;
using PaymentProcessor.Api.Infrastructure.MessageBroker;

namespace PaymentProcessor.Api.Features.PaymentProcessor;

public static class PaymentsEndpoints
{
    public static void MapPayments(this WebApplication app)
    {
        app.MapPost("/payments", async (
            MessageQueue<PaymentRequest> requestQueue,
            PaymentRequest request) =>
        {
            try
            {
                await requestQueue.EnqueueAsync(request);

                return Results.Ok();
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex);
            }
        });
    }

    public static void MapSummaries(this WebApplication app)
    {
        app.MapGet("/payments-summary", async () => "HelloWorld");
    }
}
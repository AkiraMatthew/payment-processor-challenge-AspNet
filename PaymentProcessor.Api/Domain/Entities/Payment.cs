using PaymentProcessor.Api.Infrastructure.Enum;
using System.Text.Json.Serialization;

namespace PaymentProcessor.Api.Domain.Entities;

public sealed class Payment
{
    public Guid CorrelationId { get; set; }
    public decimal Amount { get; set; }
    public DateTime RequestedAt { get;set; }

    [JsonIgnore]
    public PaymentGateway Gateway { get; set; }
}

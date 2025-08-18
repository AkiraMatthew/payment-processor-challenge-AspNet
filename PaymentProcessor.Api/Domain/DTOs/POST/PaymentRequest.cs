using PaymentProcessor.Api.Infrastructure.Enum;

namespace PaymentProcessor.Api.Domain.DTOs.POST;

public record PaymentRequest(
    Guid CorrelationId,
    decimal Amount,
    DateTime? RequestedAt,
    PaymentGateway? Gateway
);

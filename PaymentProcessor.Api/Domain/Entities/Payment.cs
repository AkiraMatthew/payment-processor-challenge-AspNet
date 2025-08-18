using PaymentProcessor.Api.Infrastructure.Enum;

namespace PaymentProcessor.Api.Domain.Entities;

public record Payment
    (
        Guid Correlation_Id,
        decimal Amount,
        PaymentGateway Gateway,
        DateTime? RequestedAt
    );

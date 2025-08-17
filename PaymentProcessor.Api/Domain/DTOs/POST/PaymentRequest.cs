namespace PaymentProcessor.Api.Domain.DTOs.POST;

public record PaymentRequest(
    Guid CorrelationId,
    decimal Amount,
    DateTime? ProcessedAt,
    string? ProcessorType
);

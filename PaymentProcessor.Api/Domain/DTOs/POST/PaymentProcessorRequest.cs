namespace PaymentProcessor.Api.Domain.DTOs.POST;

public record PaymentProcessorRequest(
    Guid CorrelationId,
    decimal Amount,
    DateTime ProcessedAt,
    string? ProcessorType    
);

namespace PaymentProcessor.Api.Domain.DTOs;

record PaymentProcessorDTO
(
    Guid CorrelationId,
    decimal Amount,
    DateTime ProcessedAt,
    string? ProcessorType 
);

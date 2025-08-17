namespace PaymentProcessor.Api.Domain.Entities;

public record Payments
    (
        Guid Correlation_Id,
        decimal Amount,
        string? Processor_Type,
        DateTime? Processed_At
    );

namespace PaymentProcessor.Api.Domain.Entities;

public record Payment
    (
        Guid Correlation_Id,
        decimal Amount,
        string? Processor_Type,
        DateTime? Processed_At
    );

namespace PaymentProcessor.Api.Domain.Entities;

public class Payments
{
    public Guid Correlation_Id { get; set; }
    public decimal Amount { get; set; }
    public string? Processor_Type { get; set; }
    public DateTime? Processed_At { get; set; }
}

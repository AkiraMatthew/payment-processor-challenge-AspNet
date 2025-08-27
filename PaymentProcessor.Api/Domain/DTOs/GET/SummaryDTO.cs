namespace PaymentProcessor.Api.Domain.DTOs.GET;

public sealed record SummaryDTO(
    string? Gateway,
    int TotalRequests, 
    decimal TotalAmount)
{
    public SummaryDTO() : this(
        string.Empty,
        default,
        default)
    { }
}

namespace PaymentProcessor.Api.Domain.DTOs.GET;

public sealed record SummaryItem(
    int TotalRequests,
    decimal TotalAmount);
public sealed record SummaryResponse(
    SummaryItem Default,
    SummaryItem Fallback
);

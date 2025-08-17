namespace PaymentProcessor.Api.Domain.DTOs.GET;

public record PaymentSummaryDTO(
    long TotalRequests, 
    decimal TotalAmount,
    decimal TotalFee,
    decimal FeePerTransaction
);

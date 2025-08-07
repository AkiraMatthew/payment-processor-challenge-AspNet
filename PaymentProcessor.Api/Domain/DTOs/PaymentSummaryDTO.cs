namespace PaymentProcessor.Api.Domain.DTOs;

public record PaymentSummaryDTO(
    long TotalRequests, 
    decimal TotalAmount,
    decimal TotalFee,
    decimal FeePerTransaction
);

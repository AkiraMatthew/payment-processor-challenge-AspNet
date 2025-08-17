namespace PaymentProcessor.Api.Domain.DTOs.GET;

public record PaymentsSummaryResponse(
    PaymentSummaryDTO Default,
    PaymentSummaryDTO Fallback
);

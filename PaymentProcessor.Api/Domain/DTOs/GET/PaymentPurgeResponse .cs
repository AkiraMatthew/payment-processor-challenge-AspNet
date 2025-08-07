namespace PaymentProcessor.Api.Domain.DTOs.GET;

public record PaymentSummaryResponse(
    PaymentSummaryDTO ProcessorDefault,
    PaymentSummaryDTO ProcessorFallback
);

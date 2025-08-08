namespace PaymentProcessor.Api.Domain.DTOs.GET;

public record PaymentPurgeResponse(
    PaymentSummaryDTO ProcessorDefault,
    PaymentSummaryDTO ProcessorFallback
);

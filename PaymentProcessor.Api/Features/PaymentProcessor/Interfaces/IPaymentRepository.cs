using PaymentProcessor.Api.Domain.DTOs.GET;
using PaymentProcessor.Api.Domain.DTOs.POST;
using PaymentProcessor.Api.Domain.Entities;

namespace PaymentProcessor.Api.Features.PaymentProcessor.Interfaces;

public interface IPaymentRepository
{
    Task InsertPaymentAsync(Payment paymentEntity, CancellationToken cancellationToken = default);
    Task<PaymentsSummaryResponse> GetPaymentsSummaryAsync(DateTime? from, DateTime? to, CancellationToken cancellationToken = default);
    Task<bool> ExistAsync(Guid correlationId, CancellationToken cancellationToken = default);
}

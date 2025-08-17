using PaymentProcessor.Api.Domain.DTOs.GET;
using PaymentProcessor.Api.Domain.DTOs.POST;
using PaymentProcessor.Api.Domain.Entities;

namespace PaymentProcessor.Api.Features.PaymentProcessor.Interfaces;

public interface IPaymentRepository
{
    Task<bool> ExistAsync(Guid correlationId, CancellationToken cancellationToken = default);
    Task InsertTransactionAsync(Payment paymentEntity, CancellationToken cancellationToken = default);
    Task<PaymentsSummaryResponse>
}

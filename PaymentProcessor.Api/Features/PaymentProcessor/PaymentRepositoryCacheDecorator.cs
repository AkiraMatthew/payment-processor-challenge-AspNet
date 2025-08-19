using PaymentProcessor.Api.Domain.DTOs.GET;
using PaymentProcessor.Api.Domain.Entities;
using PaymentProcessor.Api.Features.PaymentProcessor.Interfaces;
using PaymentProcessor.Api.Features.Redis;

namespace PaymentProcessor.Api.Features.PaymentProcessor;

public class PaymentRepositoryCacheDecorator : IPaymentRepository
{
    private readonly IPaymentRepository _inner;
    private readonly IRedisCacheService _redisCache;

    public PaymentRepositoryCacheDecorator(IPaymentRepository inner, IRedisCacheService redisCache)
    {
        _inner = inner;
        _redisCache = redisCache;
    }

    public async Task InsertPaymentAsync(Payment paymentEntity, CancellationToken cancellationToken = default)
    {
        await _inner.InsertPaymentAsync(paymentEntity, cancellationToken);
        // Optionally: Invalidate summary cache if you cache it
        await _redisCache.RemoveAsync("payments:summary");
    }

    public async Task<PaymentsSummaryResponse> GetPaymentsSummaryAsync(DateTimeOffset? fromUtc, DateTimeOffset? toUtc, CancellationToken cancellationToken = default)
    {
        // Only cache when no filters are applied (optional, adjust as needed)
        if (fromUtc is not null && toUtc is not null)
            return await _inner.GetPaymentsSummaryAsync(fromUtc, toUtc, cancellationToken);

        var cached = await _redisCache.GetAsync<PaymentsSummaryResponse>("payments:summary");
        if (cached is not null)
            return cached;

        var summary = await _inner.GetPaymentsSummaryAsync(fromUtc, toUtc, cancellationToken);
        await _redisCache.SetAsync("payments:summary", summary, TimeSpan.FromSeconds(5));
        return summary;
    }

    public Task<bool> ExistAsync(Guid correlationId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
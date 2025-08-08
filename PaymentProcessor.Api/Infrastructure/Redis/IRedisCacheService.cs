namespace PaymentProcessor.Api.Infrastructure.Redis;

public interface IRedisCacheService
{
    public Task SetAsync<T>(string key, T value, CancellationToken cancellationToken)
        where T : class;
    public Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        where T : class;
    public Task<T?> GetAsync<T>(string key, Func<Task<T>> factory, CancellationToken cancellationToken = default)
        where T : class;
    public Task<bool> ExistsAsync(string key);
    public Task RemoveAsync(string key, CancellationToken cancellationToken = default);
    public Task RemoveByPrefixAsync(string prefixKey, CancellationToken cancellationToken = default);
}

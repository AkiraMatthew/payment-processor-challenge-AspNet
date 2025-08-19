namespace PaymentProcessor.Api.Features.Redis;

public interface IRedisCacheService
{
    public Task SetAsync<T>(string key, T value, TimeSpan expiration)
        where T : class;
    public Task<T?> GetAsync<T>(string key)
        where T : class;
    public Task<T?> GetAsync<T>(string key, Func<Task<T>> factory)
        where T : class;
    public Task<bool> ExistsAsync(string key);
    public Task RemoveAsync(string key);
    public Task RemoveByPrefixAsync(string prefixKey);
}

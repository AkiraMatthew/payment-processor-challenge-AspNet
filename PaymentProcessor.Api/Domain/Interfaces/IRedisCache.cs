namespace PaymentProcessor.Api.Domain.Interfaces;

public interface IRedisCache
{
    Task SetAsync<T>(string key, T value, TimeSpan expiry);
    Task<T?> GetAsync<T>(string key);
    Task<bool> ExistsAsync(string key);
    Task<bool> RemoveAsync(string key);
}

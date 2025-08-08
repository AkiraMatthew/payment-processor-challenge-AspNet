
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace PaymentProcessor.Api.Infrastructure.Redis;

public class RedisCacheService(IDistributedCache distributedCache) : IRedisCacheService
{
    private static readonly ConcurrentDictionary<string, bool> CacheKeys = new();

    public async Task SetAsync<T>(string key, T value, CancellationToken cancellationToken) 
        where T : class
    {
        string cacheValue = JsonConvert.SerializeObject(value);

        await distributedCache.SetStringAsync(key, cacheValue, cancellationToken);

        CacheKeys.TryAdd(key, false);
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken) 
        where T : class
    {
        string? cachedValue = await distributedCache.GetStringAsync(
            key, 
            cancellationToken);

        if (cachedValue is null)
            return null;

        return JsonConvert.DeserializeObject<T>(cachedValue);
    }

    public async Task<T?> GetAsync<T>(string key, Func<Task<T>> factory, CancellationToken cancellationToken = default) where T : class
    {
        T? cachedValue = await GetAsync<T>(key, cancellationToken);

        if(cachedValue is not null)
            return cachedValue;

        cachedValue = await factory();

        await SetAsync(key, cachedValue, cancellationToken);

        return cachedValue;
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken)
    {
        await distributedCache.RemoveAsync(key, cancellationToken);

        CacheKeys.TryRemove(key, out bool _);
    }

    public async Task RemoveByPrefixAsync(string prefixKey, CancellationToken cancellationToken)
    {
        //foreach (var key in CacheKeys.Keys)
        //{
        //    await RemoveAsync(key, cancellationToken);
        //}

        IEnumerable<Task> task = CacheKeys
            .Keys
            .Where(k => k.StartsWith(prefixKey))
            .Select(k => RemoveAsync(k, cancellationToken));

        await Task.WhenAll(task);
    }

    public Task<bool> ExistsAsync(string key)
    {
        throw new NotImplementedException();
    }
}

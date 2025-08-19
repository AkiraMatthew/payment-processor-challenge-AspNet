
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Collections.Concurrent;

namespace PaymentProcessor.Api.Features.Redis;

public class RedisCacheService : IRedisCacheService
{
    private readonly IDatabase _db;
    private static readonly ConcurrentDictionary<string, bool> CacheKeys = new();

    public RedisCacheService(IConnectionMultiplexer connectionMultiplexer)
    {
        _db = connectionMultiplexer.GetDatabase();
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan expiration) 
        where T : class
    {
        string cacheValue = JsonConvert.SerializeObject(value);

        await _db.StringSetAsync(key, cacheValue, expiration);

        CacheKeys.TryAdd(key, false);
    }

    public async Task<T?> GetAsync<T>(string key) 
        where T : class
    {
        string? cachedValue = await _db.StringGetAsync(key);

        if (cachedValue is null)
            return null;

        return JsonConvert.DeserializeObject<T>(cachedValue);
    }

    public async Task<T?> GetAsync<T>(string key, Func<Task<T>> factory) where T : class
    {
        T? cachedValue = await GetAsync<T>(key);

        if(cachedValue is not null)
            return cachedValue;

        cachedValue = await factory();

        await SetAsync(key, cachedValue, TimeSpan.FromSeconds(5));

        return cachedValue;
    }

    public async Task RemoveAsync(string key)
    {
        await _db.KeyDeleteAsync(key);

        CacheKeys.TryRemove(key, out bool _);
    }

    public async Task RemoveByPrefixAsync(string prefixKey)
    {
        //foreach (var key in CacheKeys.Keys)
        //{
        //    await RemoveAsync(key);
        //}

        IEnumerable<Task> task = CacheKeys
            .Keys
            .Where(k => k.StartsWith(prefixKey))
            .Select(k => RemoveAsync(k));

        await Task.WhenAll(task);
    }

    public Task<bool> ExistsAsync(string key)
    {
        throw new NotImplementedException();
    }
}

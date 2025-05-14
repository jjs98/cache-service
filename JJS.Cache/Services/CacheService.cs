using JJS.Cache.Extensions;
using JJS.Cache.Interfaces;
using Microsoft.Extensions.Logging;

namespace JJS.Cache.Services;

public class CacheService(ILogger<CacheService> logger, TimeSpan? defaultExpiration) : ICacheService
{
    private readonly Dictionary<string, ICacheEntry> _cache = [];
    private readonly TimeSpan _defaultExpiration = defaultExpiration ?? TimeSpan.FromSeconds(60);

    public void Set<T>(string key, T? value)
        where T : notnull => Set(key, value, _defaultExpiration);

    public void Set<T>(string key, T? value, TimeSpan expiration)
        where T : notnull
    {
        if (value is null)
        {
            logger.LogDebug("Attempted to set null value for key: {Key}", key);
            return;
        }
        _cache[key] = new CacheEntry<T>(value, expiration);
    }

    public bool TryGet<T>(string key, out T? value)
    {
        if (_cache.TryGetValue(key, out var entry) && entry is CacheEntry<T> typedEntry)
        {
            if (entry.Expiration > DateTime.UtcNow)
            {
                logger.LogDebug("Cache hit for key: {Key}", key);
                value = typedEntry.Value;
                return true;
            }
            _cache.Remove(key);
        }
        logger.LogDebug("Cache miss for key: {Key}", key);
        value = default;
        return false;
    }

    public T? GetOrSet<T>(string key, Func<T> valueFactory)
        where T : notnull => GetOrSet(key, valueFactory, _defaultExpiration);

    public T? GetOrSet<T>(string key, Func<T> valueFactory, TimeSpan expiration)
        where T : notnull
    {
        if (TryGet(key, out T? value))
        {
            return value;
        }
        value = valueFactory();
        Set(key, value, expiration);
        return value;
    }

    public async Task<T?> GetOrSetAsync<T>(string key, Func<Task<T>> valueFactory)
        where T : notnull => await GetOrSetAsync(key, valueFactory, _defaultExpiration);

    public async Task<T?> GetOrSetAsync<T>(
        string key,
        Func<Task<T>> valueFactory,
        TimeSpan expiration
    )
        where T : notnull
    {
        if (TryGet(key, out T? value))
        {
            return value;
        }
        value = await valueFactory();
        Set(key, value, expiration);
        return value;
    }

    public void Remove(string key)
    {
        if (_cache.Remove(key))
        {
            logger.LogDebug("Removed cache entry for key: {Key}", key);
        }
        else
        {
            logger.LogDebug("No cache entry found for key: {Key}", key);
        }
    }

    public void Clear()
    {
        _cache.Clear();
        logger.LogDebug("Cleared all cache entries.");
    }

    private interface ICacheEntry
    {
        DateTime Expiration { get; }
        object UntypedValue { get; }
    }

    private class CacheEntry<T>(T value, TimeSpan expiration) : ICacheEntry
    {
        public T Value { get; set; } = value.DeepCopy();
        public DateTime Expiration { get; set; } = DateTime.UtcNow.Add(expiration);
        public object UntypedValue => Value!;
    }
}

using System.Text.Json;
using LinkShortener.Domain.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace LinkShortener.Infrastructure.Caching;

/// <summary>
/// In-memory cache servisi
/// </summary>
public class InMemoryCacheService : ICacheService
{
    private readonly IMemoryCache _cache;

    public InMemoryCacheService(IMemoryCache cache)
    {
        _cache = cache;
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        var value = _cache.Get<string>(key);
        if (value == null)
            return default;

        return JsonSerializer.Deserialize<T>(value);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
    {
        var options = new MemoryCacheEntryOptions();
        if (expiration.HasValue)
            options.AbsoluteExpirationRelativeToNow = expiration;

        var jsonValue = JsonSerializer.Serialize(value);
        _cache.Set(key, jsonValue, options);
    }

    public async Task RemoveAsync(string key)
    {
        _cache.Remove(key);
    }

    public async Task<bool> ExistsAsync(string key)
    {
        return _cache.TryGetValue(key, out _);
    }

    public async Task RefreshAsync(string key, TimeSpan? expiry = null)
    {
        if (_cache.TryGetValue(key, out var value))
        {
            var options = new MemoryCacheEntryOptions();
            if (expiry.HasValue)
            {
                options.AbsoluteExpirationRelativeToNow = expiry.Value;
            }
            _cache.Set(key, value, options);
        }
    }
} 
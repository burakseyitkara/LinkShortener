using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace LinkShortener.Infrastructure.Caching;

/// <summary>
/// Redis önbellekleme servis implementasyonu
/// </summary>
public class RedisCacheService : ICacheService
{
    private readonly IDatabase _cache;
    private readonly TimeSpan _defaultExpiry;

    public RedisCacheService(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Redis");
        var connection = ConnectionMultiplexer.Connect(connectionString);
        _cache = connection.GetDatabase();
        _defaultExpiry = TimeSpan.FromMinutes(30); // Varsayılan süre: 30 dakika
    }

    public async Task<T> GetAsync<T>(string key)
    {
        var value = await _cache.StringGetAsync(key);
        if (!value.HasValue)
            return default;

        return JsonSerializer.Deserialize<T>(value);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
    {
        var serializedValue = JsonSerializer.Serialize(value);
        await _cache.StringSetAsync(key, serializedValue, expiry ?? _defaultExpiry);
    }

    public async Task RemoveAsync(string key)
    {
        await _cache.KeyDeleteAsync(key);
    }

    public async Task<bool> ExistsAsync(string key)
    {
        return await _cache.KeyExistsAsync(key);
    }

    public async Task RefreshAsync(string key, TimeSpan? expiry = null)
    {
        await _cache.KeyExpireAsync(key, expiry ?? _defaultExpiry);
    }
} 
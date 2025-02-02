using System;
using System.Threading.Tasks;

namespace LinkShortener.Infrastructure.Caching;

/// <summary>
/// Önbellekleme servis arayüzü
/// </summary>
public interface ICacheService
{
    /// <summary>
    /// Önbellekten veri getir
    /// </summary>
    Task<T> GetAsync<T>(string key);

    /// <summary>
    /// Önbelleğe veri ekle
    /// </summary>
    Task SetAsync<T>(string key, T value, TimeSpan? expiry = null);

    /// <summary>
    /// Önbellekten veri sil
    /// </summary>
    Task RemoveAsync(string key);

    /// <summary>
    /// Önbellekte veri var mı kontrol et
    /// </summary>
    Task<bool> ExistsAsync(string key);

    /// <summary>
    /// Önbellek süresini güncelle
    /// </summary>
    Task RefreshAsync(string key, TimeSpan? expiry = null);
} 
using System;
using System.Threading.Tasks;
using LinkShortener.Domain.Entities;

namespace LinkShortener.Domain.Services;

/// <summary>
/// Link işlemleri için domain servis arayüzü
/// </summary>
public interface ILinkService
{
    /// <summary>
    /// Kısa URL oluştur
    /// </summary>
    Task<string> GenerateShortCodeAsync();

    /// <summary>
    /// URL'nin geçerli olup olmadığını kontrol et
    /// </summary>
    bool ValidateUrl(string url);

    /// <summary>
    /// Linkin aktif ve geçerli olup olmadığını kontrol et
    /// </summary>
    Task<bool> IsLinkValid(Link link);

    /// <summary>
    /// Link için yönlendirme türünü belirle (301 veya 302)
    /// </summary>
    int DetermineRedirectType(Link link);

    /// <summary>
    /// Linkin kullanım istatistiklerini güncelle
    /// </summary>
    Task UpdateLinkStatisticsAsync(Guid linkId, string ipAddress, string userAgent, string referer);

    /// <summary>
    /// Kısa URL'nin geçerli olup olmadığını kontrol et
    /// </summary>
    Task<bool> ValidateShortCode(string shortCode);
} 
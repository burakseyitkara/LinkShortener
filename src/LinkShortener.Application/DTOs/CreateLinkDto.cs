using System;

namespace LinkShortener.Application.DTOs;

/// <summary>
/// Link oluşturma için veri transfer nesnesi
/// </summary>
public class CreateLinkDto
{
    public CreateLinkDto()
    {
        OriginalUrl = string.Empty;
        Title = string.Empty;
        Description = string.Empty;
        CustomShortCode = string.Empty;
    }

    /// <summary>
    /// Orijinal URL (zorunlu)
    /// </summary>
    public string OriginalUrl { get; set; }

    /// <summary>
    /// Link başlığı (opsiyonel)
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Link açıklaması (opsiyonel)
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Linkin son kullanma tarihi (opsiyonel)
    /// </summary>
    public DateTime? ExpiresAt { get; set; }

    /// <summary>
    /// Özel kısa kod (opsiyonel)
    /// </summary>
    public string CustomShortCode { get; set; }

    /// <summary>
    /// Linki oluşturan kullanıcı ID'si (opsiyonel)
    /// </summary>
    public Guid? UserId { get; set; }
} 
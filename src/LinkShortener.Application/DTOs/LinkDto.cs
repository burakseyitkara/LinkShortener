using System;

namespace LinkShortener.Application.DTOs;

/// <summary>
/// Link veri transfer nesnesi
/// </summary>
public class LinkDto
{
    public LinkDto()
    {
        OriginalUrl = string.Empty;
        ShortCode = string.Empty;
        Title = string.Empty;
        Description = string.Empty;
    }

    /// <summary>
    /// Benzersiz tanımlayıcı
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Orijinal URL
    /// </summary>
    public string OriginalUrl { get; set; }

    /// <summary>
    /// Kısaltılmış URL kodu
    /// </summary>
    public string ShortCode { get; set; }

    /// <summary>
    /// Link başlığı
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Link açıklaması
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Linkin son kullanma tarihi
    /// </summary>
    public DateTime? ExpiresAt { get; set; }

    /// <summary>
    /// Toplam tıklanma sayısı
    /// </summary>
    public long ClickCount { get; set; }

    /// <summary>
    /// Linkin oluşturulma tarihi
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Linkin son güncellenme tarihi
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Linkin aktif olup olmadığı
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Linki oluşturan kullanıcı ID'si
    /// </summary>
    public Guid? UserId { get; set; }
} 
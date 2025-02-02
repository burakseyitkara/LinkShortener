using System;
using System.Collections.Generic;

namespace LinkShortener.Domain.Entities;

/// <summary>
/// Kısaltılmış link varlığı
/// </summary>
public class Link : BaseEntity
{
    /// <summary>
    /// Orijinal URL
    /// </summary>
    public string OriginalUrl { get; set; } = null!;

    /// <summary>
    /// Kısaltılmış URL kodu
    /// </summary>
    public string ShortCode { get; set; } = null!;

    /// <summary>
    /// Link başlığı (opsiyonel)
    /// </summary>
    public string Title { get; set; } = null!;

    /// <summary>
    /// Link açıklaması (opsiyonel)
    /// </summary>
    public string Description { get; set; } = null!;

    /// <summary>
    /// Linkin son kullanma tarihi (opsiyonel)
    /// </summary>
    public DateTime? ExpiresAt { get; set; }

    /// <summary>
    /// Toplam tıklanma sayısı
    /// </summary>
    public long ClickCount { get; set; }

    /// <summary>
    /// Linki oluşturan kullanıcı ID'si
    /// </summary>
    public Guid? UserId { get; set; }

    /// <summary>
    /// Linki oluşturan kullanıcı
    /// </summary>
    public User User { get; set; } = null!;

    /// <summary>
    /// Link tıklanma kayıtları
    /// </summary>
    public ICollection<LinkClick> Clicks { get; set; } = new List<LinkClick>();

    /// <summary>
    /// Kalıcı yönlendirme mi?
    /// </summary>
    public bool IsPermanent { get; set; }

    public Link()
    {
        Clicks = new List<LinkClick>();
    }
} 
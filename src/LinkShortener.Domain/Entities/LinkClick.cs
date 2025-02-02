using System;

namespace LinkShortener.Domain.Entities;

/// <summary>
/// Link tıklanma kaydı varlığı
/// </summary>
public class LinkClick : BaseEntity
{
    public LinkClick()
    {
        IpAddress = string.Empty;
        UserAgent = string.Empty;
        Referer = string.Empty;
        Country = string.Empty;
        City = string.Empty;
        DeviceType = string.Empty;
        OperatingSystem = string.Empty;
    }

    /// <summary>
    /// Tıklanan link ID'si
    /// </summary>
    public Guid LinkId { get; set; }

    /// <summary>
    /// Tıklanan link
    /// </summary>
    public virtual Link Link { get; set; } = null!;

    /// <summary>
    /// Tıklayan kullanıcının IP adresi
    /// </summary>
    public string IpAddress { get; set; }

    /// <summary>
    /// Kullanılan tarayıcı bilgisi
    /// </summary>
    public string UserAgent { get; set; }

    /// <summary>
    /// Referrer URL (nereden geldiği)
    /// </summary>
    public string Referer { get; set; }

    /// <summary>
    /// Tahmini ülke bilgisi
    /// </summary>
    public string Country { get; set; }

    /// <summary>
    /// Tahmini şehir bilgisi
    /// </summary>
    public string City { get; set; }

    /// <summary>
    /// Kullanılan cihaz türü (Mobil, Desktop, Tablet vb.)
    /// </summary>
    public string DeviceType { get; set; }

    /// <summary>
    /// İşletim sistemi bilgisi
    /// </summary>
    public string OperatingSystem { get; set; }
} 
using System;

namespace LinkShortener.Application.DTOs;

/// <summary>
/// Link tıklanma kaydı veri transfer nesnesi
/// </summary>
public class LinkClickDto
{
    public LinkClickDto()
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
    /// Benzersiz tanımlayıcı
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Tıklanan link ID'si
    /// </summary>
    public Guid LinkId { get; set; }

    /// <summary>
    /// Tıklayan kullanıcının IP adresi
    /// </summary>
    public string IpAddress { get; set; }

    /// <summary>
    /// Kullanılan tarayıcı bilgisi
    /// </summary>
    public string UserAgent { get; set; }

    /// <summary>
    /// Referrer URL
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
    /// Kullanılan cihaz türü
    /// </summary>
    public string DeviceType { get; set; }

    /// <summary>
    /// İşletim sistemi bilgisi
    /// </summary>
    public string OperatingSystem { get; set; }

    /// <summary>
    /// Tıklanma tarihi
    /// </summary>
    public DateTime CreatedAt { get; set; }
} 
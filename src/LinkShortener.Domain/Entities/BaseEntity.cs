using System;

namespace LinkShortener.Domain.Entities;

/// <summary>
/// Tüm varlık sınıfları için temel sınıf
/// </summary>
public abstract class BaseEntity
{
    /// <summary>
    /// Benzersiz tanımlayıcı
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Oluşturulma tarihi
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Son güncelleme tarihi
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Silinme tarihi (soft delete için)
    /// </summary>
    public DateTime? DeletedAt { get; set; }

    /// <summary>
    /// Aktif/Pasif durumu
    /// </summary>
    public bool IsActive { get; set; } = true;
} 
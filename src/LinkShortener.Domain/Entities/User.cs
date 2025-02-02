using System;
using System.Collections.Generic;

namespace LinkShortener.Domain.Entities;

/// <summary>
/// Kullanıcı varlığı
/// </summary>
public class User : BaseEntity
{
    /// <summary>
    /// Kullanıcı adı
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// E-posta adresi
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Şifre (hash'lenmiş)
    /// </summary>
    public string PasswordHash { get; set; }

    /// <summary>
    /// Ad
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// Soyad
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    /// E-posta doğrulanma durumu
    /// </summary>
    public bool IsEmailVerified { get; set; }

    /// <summary>
    /// Son giriş tarihi
    /// </summary>
    public DateTime? LastLoginAt { get; set; }

    /// <summary>
    /// Kullanıcının oluşturduğu linkler
    /// </summary>
    public virtual ICollection<Link> Links { get; set; }

    public User()
    {
        Username = string.Empty;
        Email = string.Empty;
        PasswordHash = string.Empty;
        FirstName = string.Empty;
        LastName = string.Empty;
        Links = new List<Link>();
    }
} 
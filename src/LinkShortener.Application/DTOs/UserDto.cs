using System;

namespace LinkShortener.Application.DTOs;

/// <summary>
/// Kullanıcı veri transfer nesnesi
/// </summary>
public class UserDto
{
    public UserDto()
    {
        Username = string.Empty;
        Email = string.Empty;
        FirstName = string.Empty;
        LastName = string.Empty;
    }

    /// <summary>
    /// Benzersiz tanımlayıcı
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Kullanıcı adı
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// E-posta adresi
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Ad
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// Soyad
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    /// Tam ad
    /// </summary>
    public string FullName => $"{FirstName} {LastName}".Trim();

    /// <summary>
    /// E-posta doğrulanma durumu
    /// </summary>
    public bool IsEmailVerified { get; set; }

    /// <summary>
    /// Son giriş tarihi
    /// </summary>
    public DateTime? LastLoginAt { get; set; }

    /// <summary>
    /// Kullanıcının oluşturulma tarihi
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Kullanıcının aktif olup olmadığı
    /// </summary>
    public bool IsActive { get; set; }
} 
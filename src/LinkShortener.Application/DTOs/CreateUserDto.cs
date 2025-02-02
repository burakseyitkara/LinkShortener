namespace LinkShortener.Application.DTOs;

/// <summary>
/// Kullanıcı oluşturma için veri transfer nesnesi
/// </summary>
public class CreateUserDto
{
    public CreateUserDto()
    {
        Username = string.Empty;
        Email = string.Empty;
        Password = string.Empty;
        ConfirmPassword = string.Empty;
        FirstName = string.Empty;
        LastName = string.Empty;
    }

    /// <summary>
    /// Kullanıcı adı (zorunlu)
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// E-posta adresi (zorunlu)
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Şifre (zorunlu)
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// Şifre tekrarı (zorunlu)
    /// </summary>
    public string ConfirmPassword { get; set; }

    /// <summary>
    /// Ad (zorunlu)
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// Soyad (zorunlu)
    /// </summary>
    public string LastName { get; set; }
} 
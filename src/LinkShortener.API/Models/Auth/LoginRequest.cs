using System.ComponentModel.DataAnnotations;

namespace LinkShortener.API.Models.Auth;

/// <summary>
/// Giriş isteği modeli
/// </summary>
public class LoginRequest
{
    public LoginRequest()
    {
        UsernameOrEmail = string.Empty;
        Password = string.Empty;
    }

    /// <summary>
    /// Kullanıcı adı veya e-posta
    /// </summary>
    [Required(ErrorMessage = "Kullanıcı adı veya e-posta gereklidir.")]
    public string UsernameOrEmail { get; set; }

    /// <summary>
    /// Şifre
    /// </summary>
    [Required(ErrorMessage = "Şifre gereklidir.")]
    public string Password { get; set; }
} 
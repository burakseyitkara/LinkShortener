using LinkShortener.Application.DTOs;

namespace LinkShortener.API.Models.Auth;

/// <summary>
/// Giriş yanıt modeli
/// </summary>
public class LoginResponse
{
    public LoginResponse()
    {
        Token = string.Empty;
        User = new UserDto();
    }

    /// <summary>
    /// JWT token
    /// </summary>
    public string Token { get; set; }

    /// <summary>
    /// Token geçerlilik süresi (dakika)
    /// </summary>
    public int ExpiresIn { get; set; }

    /// <summary>
    /// Kullanıcı bilgileri
    /// </summary>
    public UserDto User { get; set; }
} 
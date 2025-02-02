using System.Security.Claims;
using LinkShortener.Application.DTOs;

namespace LinkShortener.API.Services.Auth;

/// <summary>
/// JWT işlemleri için servis arayüzü
/// </summary>
public interface IJwtService
{
    /// <summary>
    /// Kullanıcı için JWT token oluştur
    /// </summary>
    string GenerateToken(UserDto user);

    /// <summary>
    /// JWT token'dan kullanıcı bilgilerini al
    /// </summary>
    ClaimsPrincipal GetPrincipalFromToken(string token);

    /// <summary>
    /// Token'ın geçerli olup olmadığını kontrol et
    /// </summary>
    bool ValidateToken(string token);
} 
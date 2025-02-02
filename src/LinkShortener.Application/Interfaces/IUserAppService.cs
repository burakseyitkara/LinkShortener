using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LinkShortener.Application.DTOs;

namespace LinkShortener.Application.Interfaces;

/// <summary>
/// Kullanıcı işlemleri için uygulama servis arayüzü
/// </summary>
public interface IUserAppService
{
    /// <summary>
    /// Yeni kullanıcı oluştur (kayıt)
    /// </summary>
    Task<UserDto> RegisterAsync(CreateUserDto input);

    /// <summary>
    /// Kullanıcı girişi
    /// </summary>
    Task<UserDto> LoginAsync(string usernameOrEmail, string password);

    /// <summary>
    /// Kullanıcı bilgilerini güncelle
    /// </summary>
    Task<UserDto> UpdateAsync(Guid id, CreateUserDto input);

    /// <summary>
    /// Kullanıcı sil
    /// </summary>
    Task DeleteAsync(Guid id);

    /// <summary>
    /// ID'ye göre kullanıcı getir
    /// </summary>
    Task<UserDto> GetByIdAsync(Guid id);

    /// <summary>
    /// Kullanıcı adına göre kullanıcı getir
    /// </summary>
    Task<UserDto> GetByUsernameAsync(string username);

    /// <summary>
    /// E-posta adresine göre kullanıcı getir
    /// </summary>
    Task<UserDto> GetByEmailAsync(string email);

    /// <summary>
    /// E-posta doğrulama
    /// </summary>
    Task VerifyEmailAsync(string email, string token);

    /// <summary>
    /// Şifre sıfırlama e-postası gönder
    /// </summary>
    Task SendPasswordResetEmailAsync(string email);

    /// <summary>
    /// Şifre sıfırla
    /// </summary>
    Task ResetPasswordAsync(string email, string token, string newPassword);

    /// <summary>
    /// Tüm kullanıcıları getir
    /// </summary>
    Task<IEnumerable<UserDto>> GetAllAsync();
} 
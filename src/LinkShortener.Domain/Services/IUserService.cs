using System.Threading.Tasks;

namespace LinkShortener.Domain.Services;

/// <summary>
/// Kullanıcı işlemleri için domain servis arayüzü
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Şifreyi hash'le
    /// </summary>
    Task<string> HashPasswordAsync(string password);

    /// <summary>
    /// Şifre hash'ini doğrula
    /// </summary>
    Task<bool> VerifyPasswordAsync(string password, string passwordHash);

    /// <summary>
    /// E-posta adresinin geçerli olup olmadığını kontrol et
    /// </summary>
    bool ValidateEmail(string email);

    /// <summary>
    /// Kullanıcı adının geçerli olup olmadığını kontrol et
    /// </summary>
    bool ValidateUsername(string username);

    /// <summary>
    /// E-posta doğrulama token'ı oluştur
    /// </summary>
    Task<string> GenerateEmailVerificationTokenAsync(string email);

    /// <summary>
    /// Şifre sıfırlama token'ı oluştur
    /// </summary>
    Task<string> GeneratePasswordResetTokenAsync(string email);

    Task<bool> ValidatePassword(string password);
} 
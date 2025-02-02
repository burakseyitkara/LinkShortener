using System.Threading.Tasks;
using LinkShortener.Domain.Entities;

namespace LinkShortener.Domain.Interfaces;

/// <summary>
/// Kullanıcı repository arayüzü
/// </summary>
public interface IUserRepository : IRepository<User>
{
    /// <summary>
    /// Kullanıcı adına göre kullanıcı getir
    /// </summary>
    Task<User> GetByUsernameAsync(string username);

    /// <summary>
    /// E-posta adresine göre kullanıcı getir
    /// </summary>
    Task<User> GetByEmailAsync(string email);

    /// <summary>
    /// Kullanıcı adı veya e-posta ile giriş kontrolü
    /// </summary>
    Task<User> ValidateCredentialsAsync(string usernameOrEmail);

    /// <summary>
    /// E-posta doğrulama durumunu güncelle
    /// </summary>
    Task UpdateEmailVerificationStatusAsync(string email, bool isVerified);

    /// <summary>
    /// Son giriş tarihini güncelle
    /// </summary>
    Task UpdateLastLoginAsync(string username);
} 
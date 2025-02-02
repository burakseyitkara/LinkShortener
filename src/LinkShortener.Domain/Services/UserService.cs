using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

namespace LinkShortener.Domain.Services;

/// <summary>
/// Kullanıcı servisi implementasyonu
/// </summary>
public class UserService : IUserService
{
    private const int SaltRounds = 12;

    public async Task<string> HashPasswordAsync(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password, SaltRounds);
    }

    public async Task<bool> VerifyPasswordAsync(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }

    public async Task<bool> ValidatePassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            return false;

        // En az 8 karakter
        if (password.Length < 8)
            return false;

        // En az bir büyük harf
        if (!password.Any(char.IsUpper))
            return false;

        // En az bir küçük harf
        if (!password.Any(char.IsLower))
            return false;

        // En az bir rakam
        if (!password.Any(char.IsDigit))
            return false;

        // En az bir özel karakter
        if (!password.Any(ch => !char.IsLetterOrDigit(ch)))
            return false;

        return true;
    }

    public bool ValidateEmail(string email)
    {
        if (string.IsNullOrEmpty(email))
            return false;

        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    public bool ValidateUsername(string username)
    {
        if (string.IsNullOrEmpty(username) || username.Length < 3 || username.Length > 50)
            return false;

        var validUsernameRegex = new Regex("^[a-zA-Z0-9_-]+$");
        return validUsernameRegex.IsMatch(username);
    }

    public async Task<string> GenerateEmailVerificationTokenAsync(string email)
    {
        var tokenBytes = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(tokenBytes);
        return Convert.ToBase64String(tokenBytes);
    }

    public async Task<string> GeneratePasswordResetTokenAsync(string email)
    {
        var tokenBytes = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(tokenBytes);
        return Convert.ToBase64String(tokenBytes);
    }
} 
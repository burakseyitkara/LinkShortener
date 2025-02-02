namespace LinkShortener.API.Services.Auth;

/// <summary>
/// JWT ayarları
/// </summary>
public class JwtSettings
{
    /// <summary>
    /// JWT için gizli anahtar
    /// </summary>
    public string SecretKey { get; set; } = null!;

    /// <summary>
    /// Token geçerlilik süresi (dakika)
    /// </summary>
    public int ExpiryMinutes { get; set; }

    /// <summary>
    /// Token yayıncısı
    /// </summary>
    public string Issuer { get; set; } = null!;

    /// <summary>
    /// Token hedef kitlesi
    /// </summary>
    public string Audience { get; set; } = null!;
} 
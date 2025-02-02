using System.Security.Cryptography;
using System.Text;
using LinkShortener.Domain.Entities;
using LinkShortener.Domain.Interfaces;
using LinkShortener.Domain.Exceptions;

namespace LinkShortener.Domain.Services;

/// <summary>
/// Link servisi implementasyonu
/// </summary>
public class LinkService : ILinkService
{
    private const string AllowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    private const int DefaultShortCodeLength = 6;
    private readonly ILinkRepository _linkRepository;

    public LinkService(ILinkRepository linkRepository)
    {
        _linkRepository = linkRepository;
    }

    public async Task<string> GenerateShortCodeAsync()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var random = new Random();
        var shortCode = new string(Enumerable.Repeat(chars, DefaultShortCodeLength)
            .Select(s => s[random.Next(s.Length)]).ToArray());

        // Eğer üretilen kod zaten kullanılıyorsa, yeni bir tane üret
        while (await _linkRepository.GetByShortCodeAsync(shortCode) != null)
        {
            shortCode = new string(Enumerable.Repeat(chars, DefaultShortCodeLength)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        return shortCode;
    }

    public bool ValidateUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
            && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }

    public async Task<bool> ValidateShortCode(string shortCode)
    {
        if (string.IsNullOrWhiteSpace(shortCode))
            return false;

        // Sadece alfanümerik karakterler ve tire içerebilir
        if (!shortCode.All(c => char.IsLetterOrDigit(c) || c == '-'))
            return false;

        // En az 3, en fazla 50 karakter
        if (shortCode.Length < 3 || shortCode.Length > 50)
            return false;

        // Başka bir link tarafından kullanılmıyor olmalı
        var existingLink = await _linkRepository.GetByShortCodeAsync(shortCode);
        return existingLink == null;
    }

    public bool IsExpired(Link link)
    {
        return link.ExpiresAt.HasValue && link.ExpiresAt.Value <= DateTime.UtcNow;
    }

    public async Task<bool> IsLinkValid(Link link)
    {
        if (link == null)
            return false;

        if (!link.IsActive)
            return false;

        if (link.ExpiresAt.HasValue && link.ExpiresAt.Value < DateTime.UtcNow)
            return false;

        return true;
    }

    public int DetermineRedirectType(Link link)
    {
        // Kalıcı yönlendirme için 301, geçici yönlendirme için 302 kullan
        return link.IsPermanent ? 301 : 302;
    }

    public async Task UpdateLinkStatisticsAsync(Guid linkId, string ipAddress, string userAgent, string referer)
    {
        var link = await _linkRepository.GetByIdAsync(linkId);
        if (link == null)
            throw new NotFoundException("Link not found");

        link.ClickCount++;
        await _linkRepository.UpdateAsync(link);
    }
} 
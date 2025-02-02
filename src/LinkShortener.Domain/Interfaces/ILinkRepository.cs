using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LinkShortener.Domain.Entities;

namespace LinkShortener.Domain.Interfaces;

/// <summary>
/// Link repository arayüzü
/// </summary>
public interface ILinkRepository : IRepository<Link>
{
    /// <summary>
    /// Kısa koda göre link getir
    /// </summary>
    Task<Link> GetByShortCodeAsync(string shortCode);

    /// <summary>
    /// Kullanıcıya ait linkleri getir
    /// </summary>
    Task<IEnumerable<Link>> GetByUserIdAsync(Guid userId);

    /// <summary>
    /// Süresi dolmuş linkleri getir
    /// </summary>
    Task<IEnumerable<Link>> GetExpiredLinksAsync();

    /// <summary>
    /// En çok tıklanan linkleri getir
    /// </summary>
    Task<IEnumerable<Link>> GetMostClickedLinksAsync(int count);

    /// <summary>
    /// Link tıklanma sayısını artır
    /// </summary>
    Task IncrementClickCountAsync(Guid linkId);
} 
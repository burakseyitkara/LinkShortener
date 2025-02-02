using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LinkShortener.Application.DTOs;

namespace LinkShortener.Application.Interfaces;

/// <summary>
/// Link işlemleri için uygulama servis arayüzü
/// </summary>
public interface ILinkAppService
{
    /// <summary>
    /// Yeni link oluştur
    /// </summary>
    Task<LinkDto> CreateAsync(CreateLinkDto input);

    /// <summary>
    /// Link güncelle
    /// </summary>
    Task<LinkDto> UpdateAsync(Guid id, CreateLinkDto input);

    /// <summary>
    /// Link sil
    /// </summary>
    Task DeleteAsync(Guid id);

    /// <summary>
    /// ID'ye göre link getir
    /// </summary>
    Task<LinkDto> GetByIdAsync(Guid id);

    /// <summary>
    /// Kısa koda göre link getir
    /// </summary>
    Task<LinkDto> GetByShortCodeAsync(string shortCode);

    /// <summary>
    /// Kullanıcıya ait linkleri getir
    /// </summary>
    Task<IEnumerable<LinkDto>> GetByUserIdAsync(Guid userId);

    /// <summary>
    /// En çok tıklanan linkleri getir
    /// </summary>
    Task<IEnumerable<LinkDto>> GetMostClickedLinksAsync(int count);

    /// <summary>
    /// Link tıklanma olayını kaydet
    /// </summary>
    Task RecordClickAsync(string shortCode, string ipAddress, string userAgent, string referer);

    /// <summary>
    /// Link istatistiklerini getir
    /// </summary>
    Task<IEnumerable<LinkClickDto>> GetClickStatisticsAsync(Guid linkId, DateTime? startDate = null, DateTime? endDate = null);
} 
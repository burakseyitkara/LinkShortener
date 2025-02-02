using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LinkShortener.Domain.Entities;

namespace LinkShortener.Domain.Interfaces;

/// <summary>
/// Link tıklanma kaydı repository arayüzü
/// </summary>
public interface ILinkClickRepository : IRepository<LinkClick>
{
    /// <summary>
    /// Link ID'sine göre tıklanma kayıtlarını getir
    /// </summary>
    Task<IEnumerable<LinkClick>> GetByLinkIdAsync(Guid linkId);

    /// <summary>
    /// Link ID'sine göre tıklanma kayıtlarını getir
    /// </summary>
    Task<IEnumerable<LinkClick>> GetClicksByLinkIdAsync(Guid linkId, DateTime? startDate = null, DateTime? endDate = null);

    /// <summary>
    /// Belirli bir tarih aralığındaki tıklanma kayıtlarını getir
    /// </summary>
    Task<IEnumerable<LinkClick>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);

    /// <summary>
    /// IP adresine göre tıklanma kayıtlarını getir
    /// </summary>
    Task<IEnumerable<LinkClick>> GetByIpAddressAsync(string ipAddress);

    /// <summary>
    /// Ülkeye göre tıklanma kayıtlarını getir
    /// </summary>
    Task<IEnumerable<LinkClick>> GetByCountryAsync(string country);

    /// <summary>
    /// Cihaz türüne göre tıklanma kayıtlarını getir
    /// </summary>
    Task<IEnumerable<LinkClick>> GetByDeviceTypeAsync(string deviceType);
} 
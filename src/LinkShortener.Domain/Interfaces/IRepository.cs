using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LinkShortener.Domain.Entities;

namespace LinkShortener.Domain.Interfaces;

/// <summary>
/// Generic repository arayüzü
/// </summary>
/// <typeparam name="T">Varlık tipi</typeparam>
public interface IRepository<T> where T : BaseEntity
{
    /// <summary>
    /// Varlığı ID'ye göre getir
    /// </summary>
    Task<T> GetByIdAsync(Guid id);

    /// <summary>
    /// Tüm varlıkları getir
    /// </summary>
    Task<IEnumerable<T>> GetAllAsync();

    /// <summary>
    /// Belirtilen koşula göre varlıkları getir
    /// </summary>
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Yeni varlık ekle
    /// </summary>
    Task<T> AddAsync(T entity);

    /// <summary>
    /// Varlığı güncelle
    /// </summary>
    Task UpdateAsync(T entity);

    /// <summary>
    /// Varlığı sil
    /// </summary>
    Task DeleteAsync(T entity);

    /// <summary>
    /// Belirtilen koşula göre varlık var mı kontrol et
    /// </summary>
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
} 
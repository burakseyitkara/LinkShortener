using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinkShortener.Domain.Entities;
using LinkShortener.Domain.Interfaces;
using LinkShortener.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace LinkShortener.Infrastructure.Data.Repositories;

/// <summary>
/// Link tıklanma kaydı repository implementasyonu
/// </summary>
public class LinkClickRepository : Repository<LinkClick>, ILinkClickRepository
{
    public LinkClickRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<LinkClick>> GetByLinkIdAsync(Guid linkId)
    {
        return await _context.LinkClicks
            .Where(lc => lc.LinkId == linkId)
            .OrderByDescending(lc => lc.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<LinkClick>> GetClicksByLinkIdAsync(Guid linkId, DateTime? startDate = null, DateTime? endDate = null)
    {
        var query = _context.LinkClicks.Where(lc => lc.LinkId == linkId);

        if (startDate.HasValue)
            query = query.Where(lc => lc.CreatedAt >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(lc => lc.CreatedAt <= endDate.Value);

        return await query
            .OrderByDescending(lc => lc.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<LinkClick>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.LinkClicks
            .Where(lc => lc.CreatedAt >= startDate && lc.CreatedAt <= endDate)
            .OrderByDescending(lc => lc.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<LinkClick>> GetByIpAddressAsync(string ipAddress)
    {
        return await _dbSet
            .Where(x => x.IpAddress == ipAddress)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<LinkClick>> GetByCountryAsync(string country)
    {
        return await _dbSet
            .Where(x => x.Country == country)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<LinkClick>> GetByDeviceTypeAsync(string deviceType)
    {
        return await _dbSet
            .Where(x => x.DeviceType == deviceType)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    public override async Task<LinkClick> GetByIdAsync(Guid id)
    {
        return await _dbSet
            .Include(x => x.Link)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
} 
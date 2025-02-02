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
/// Link repository implementasyonu
/// </summary>
public class LinkRepository : Repository<Link>, ILinkRepository
{
    public LinkRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Link> GetByShortCodeAsync(string shortCode)
    {
        return await _dbSet
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.ShortCode == shortCode);
    }

    public async Task<IEnumerable<Link>> GetByUserIdAsync(Guid userId)
    {
        return await _dbSet
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Link>> GetExpiredLinksAsync()
    {
        var now = DateTime.UtcNow;
        return await _dbSet
            .Where(x => x.ExpiresAt.HasValue && x.ExpiresAt.Value < now)
            .ToListAsync();
    }

    public async Task<IEnumerable<Link>> GetMostClickedLinksAsync(int count)
    {
        return await _dbSet
            .OrderByDescending(x => x.ClickCount)
            .Take(count)
            .ToListAsync();
    }

    public async Task IncrementClickCountAsync(Guid linkId)
    {
        await _context.Database.ExecuteSqlRawAsync(
            "UPDATE Links SET ClickCount = ClickCount + 1 WHERE Id = {0}", linkId);
    }

    public override async Task<Link> GetByIdAsync(Guid id)
    {
        return await _dbSet
            .Include(x => x.User)
            .Include(x => x.Clicks)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
} 
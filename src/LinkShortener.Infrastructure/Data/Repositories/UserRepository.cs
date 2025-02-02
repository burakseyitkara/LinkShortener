using System;
using System.Threading.Tasks;
using LinkShortener.Domain.Entities;
using LinkShortener.Domain.Interfaces;
using LinkShortener.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace LinkShortener.Infrastructure.Data.Repositories;

/// <summary>
/// Kullanıcı repository implementasyonu
/// </summary>
public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<User> GetByUsernameAsync(string username)
    {
        return await _dbSet
            .Include(x => x.Links)
            .FirstOrDefaultAsync(x => x.Username == username && x.DeletedAt == null);
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        return await _dbSet
            .Include(x => x.Links)
            .FirstOrDefaultAsync(x => x.Email == email && x.DeletedAt == null);
    }

    public async Task<User> ValidateCredentialsAsync(string usernameOrEmail)
    {
        return await _dbSet
            .FirstOrDefaultAsync(x =>
                (x.Username == usernameOrEmail || x.Email == usernameOrEmail) &&
                x.DeletedAt == null);
    }

    public async Task UpdateEmailVerificationStatusAsync(string email, bool isVerified)
    {
        var user = await GetByEmailAsync(email);
        if (user != null)
        {
            user.IsEmailVerified = isVerified;
            await UpdateAsync(user);
        }
    }

    public async Task UpdateLastLoginAsync(string username)
    {
        var user = await GetByUsernameAsync(username);
        if (user != null)
        {
            user.LastLoginAt = DateTime.UtcNow;
            await UpdateAsync(user);
        }
    }

    public override async Task<User> GetByIdAsync(Guid id)
    {
        return await _dbSet
            .Include(x => x.Links)
            .FirstOrDefaultAsync(x => x.Id == id && x.DeletedAt == null);
    }
} 
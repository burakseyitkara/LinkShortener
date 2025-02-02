using System;
using System.Threading.Tasks;
using LinkShortener.Domain.Entities;
using LinkShortener.Domain.Services;
using LinkShortener.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LinkShortener.Infrastructure.Data.Seeds;

/// <summary>
/// Örnek veritabanı verilerini oluşturan sınıf
/// </summary>
public class DataSeeder
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<DataSeeder> _logger;

    public DataSeeder(
        IServiceProvider serviceProvider,
        ILogger<DataSeeder> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var userService = scope.ServiceProvider.GetRequiredService<IUserService>();

            // Veritabanını oluştur
            await context.Database.MigrateAsync();

            // Örnek veriler zaten varsa çık
            if (await context.Users.AnyAsync())
            {
                return;
            }

            _logger.LogInformation("Örnek veriler oluşturuluyor...");

            // Örnek kullanıcı
            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = "demo",
                Email = "demo@example.com",
                PasswordHash = await userService.HashPasswordAsync("Demo123!"),
                FirstName = "Demo",
                LastName = "User",
                IsEmailVerified = true,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await context.Users.AddAsync(user);

            // Örnek linkler
            var links = new[]
            {
                new Link
                {
                    Id = Guid.NewGuid(),
                    OriginalUrl = "https://www.google.com",
                    ShortCode = "google",
                    Title = "Google",
                    Description = "Google arama motoru",
                    UserId = user.Id,
                    ClickCount = 0,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new Link
                {
                    Id = Guid.NewGuid(),
                    OriginalUrl = "https://www.github.com",
                    ShortCode = "github",
                    Title = "GitHub",
                    Description = "GitHub kod deposu",
                    UserId = user.Id,
                    ClickCount = 0,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                }
            };

            await context.Links.AddRangeAsync(links);

            // Örnek tıklanma kayıtları
            var clicks = new[]
            {
                new LinkClick
                {
                    Id = Guid.NewGuid(),
                    LinkId = links[0].Id,
                    IpAddress = "127.0.0.1",
                    UserAgent = "Mozilla/5.0",
                    Country = "Turkey",
                    City = "Istanbul",
                    DeviceType = "Desktop",
                    OperatingSystem = "Windows",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new LinkClick
                {
                    Id = Guid.NewGuid(),
                    LinkId = links[1].Id,
                    IpAddress = "127.0.0.1",
                    UserAgent = "Mozilla/5.0",
                    Country = "Turkey",
                    City = "Ankara",
                    DeviceType = "Mobile",
                    OperatingSystem = "Android",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                }
            };

            await context.LinkClicks.AddRangeAsync(clicks);

            // Değişiklikleri kaydet
            await context.SaveChangesAsync();

            _logger.LogInformation("Örnek veriler başarıyla oluşturuldu.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Örnek veriler oluşturulurken hata oluştu.");
            throw;
        }
    }
} 
using LinkShortener.Domain.Interfaces;
using LinkShortener.Infrastructure.Caching;
using LinkShortener.Infrastructure.Data.Context;
using LinkShortener.Infrastructure.Data.Repositories;
using LinkShortener.Infrastructure.Data.Seeds;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LinkShortener.Infrastructure;

/// <summary>
/// Infrastructure katmanı için DI kayıtları
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // DbContext
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));

        // Repositories
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<ILinkRepository, LinkRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ILinkClickRepository, LinkClickRepository>();

        // Cache Service
        var redisConnection = configuration.GetConnectionString("Redis");
        if (!string.IsNullOrEmpty(redisConnection))
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConnection;
                options.InstanceName = "LinkShortener_";
            });
            services.AddScoped<ICacheService, RedisCacheService>();
        }
        else
        {
            services.AddMemoryCache();
            services.AddScoped<ICacheService, InMemoryCacheService>();
        }

        // Data Seeder
        services.AddScoped<DataSeeder>();

        return services;
    }
} 
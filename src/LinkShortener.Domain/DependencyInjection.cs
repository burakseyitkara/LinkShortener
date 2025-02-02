using LinkShortener.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LinkShortener.Domain;

/// <summary>
/// Domain katmanı için DI kayıtları
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services.AddScoped<ILinkService, LinkService>();
        services.AddScoped<IUserService, UserService>();

        return services;
    }
} 
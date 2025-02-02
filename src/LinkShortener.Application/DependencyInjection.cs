using System.Reflection;
using FluentValidation;
using LinkShortener.Application.Interfaces;
using LinkShortener.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LinkShortener.Application;

/// <summary>
/// Application katmanı için DI kayıtları
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // AutoMapper
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        // FluentValidation
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        // Application Services
        services.AddScoped<ILinkAppService, LinkAppService>();
        services.AddScoped<IUserAppService, UserAppService>();

        return services;
    }
} 
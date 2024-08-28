using System.Reflection;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Application.Extensions;
using MyAtelier.DAL.Context;
using MyAtelier.DAL.Entities;

namespace Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentationLayer
        (this IServiceCollection services, ConfigurationManager config)
    {
        AddMappingConfig(services);
        AddIdentity(services);
        AddAuthentication(services);
        AddControllersWithViews(services);
        
        return services;
    }

    private static void AddControllersWithViews(IServiceCollection services)
    {
        services.AddControllersWithViews();
    }
    
    private static void AddMappingConfig(IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        
        config.Scan(Assembly.GetExecutingAssembly());
        services.AddSingleton(config);
        services.AddSingleton<IMapper, ServiceMapper>();
    }

    private static void AddIdentity(IServiceCollection services)
    {
        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.SignIn.RequireConfirmedAccount = false;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddErrorDescriber<AppErrorDescriber>();
    }

    private static void AddAuthentication(IServiceCollection services)
    {
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.RequireAuthenticatedSignIn = false;
            })
            .AddCookie(options =>
            {
                options.LoginPath = "/authentication/login";
                options.AccessDeniedPath = "/authentication/access-denied";
            });
    }
}
using Application.Common.AccountConfirmation;
using Application.Services.Implementation;
using Application.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Application.Config.Authentication;
using Microsoft.Extensions.Options;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddBusinessLayer(this IServiceCollection services, ConfigurationManager config)
    {
        AddLogicServices(services);
        AddEmailSender(services, config);
        AddCodeConfirmationGenerator(services, config);
        
        return services;
    }

    private static void AddLogicServices(IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IClothingService, ClothingService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IMaterialService, MaterialService>();
        services.AddScoped<IFavorService, FavorService>();
        services.AddScoped<IOrderService, OrderService>();
    }

    private static void AddEmailSender(IServiceCollection services, ConfigurationManager configuration)
    {
        services.Configure<EmailMessageSenderOptions>(configuration.GetSection("EmailSenderOptions"));

        using (var p = services.BuildServiceProvider())
        {
            var conf = p.GetRequiredService<IOptions<EmailMessageSenderOptions>>().Value;
            Console.WriteLine(conf.Email);
            Console.WriteLine(conf.Password);
        }
        
        services.AddScoped<IAsyncMessageSender, EmailAsyncMessageSender>();
    }

    private static void AddCodeConfirmationGenerator(IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddScoped<ICodeGenerator<int>, NumericCodeGenerator>();
    }
}
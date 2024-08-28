using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyAtelier.DAL.Context;
using MyAtelier.DAL.Unit.Implementation;
using MyAtelier.DAL.Unit.Interfaces;

namespace MyAtelier.DAL;

public static class DependencyInjection
{
    public static IServiceCollection AddDataAccessLayer(
        this IServiceCollection services, 
        ConfigurationManager configurationManager)
    {
        AddDbContext(services, configurationManager);
        AddUnitOfWork(services);
        return services;
    }

    private static void AddDbContext(IServiceCollection services, ConfigurationManager configurationManager)
    {
        var connectionString = configurationManager.GetConnectionString("DefaultConnectionString");
        Console.WriteLine(connectionString);
        
        services.AddDbContext<AppDbContext>(
            options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
    }

    private static void AddUnitOfWork(IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}
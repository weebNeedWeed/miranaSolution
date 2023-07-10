using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using miranaSolution.Data.Main;

namespace miranaSolution.Data;

public static class DependencyInjection
{
    public static IServiceCollection AddDataLayer(this IServiceCollection services)
    {
        services.ConfigureOptions<DatabaseOptionsSetup>();

        services.AddDbContext<MiranaDbContext>((serviceProvider, options) =>
        {
            var databaseOptions = serviceProvider.GetService<IOptions<DatabaseOptions>>()!.Value;

            options.UseSqlServer(
                databaseOptions.ConnectionString,
                x =>
                {
                    x.MigrationsAssembly(typeof(DependencyInjection).Assembly.FullName);
                    x.EnableRetryOnFailure(databaseOptions.MaxRetryCount);
                    x.CommandTimeout(databaseOptions.CommandTimeout);
                });

            options.EnableDetailedErrors(databaseOptions.EnableDetailedErrors);
            options.EnableSensitiveDataLogging(databaseOptions.EnableSensitiveDataLogging);
        });
        
        services.AddScoped<DbContext, MiranaDbContext>();

        return services;
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using miranaSolution.Data.Main;

namespace miranaSolution.Data;

public static class DependencyInjection
{
    public static IServiceCollection AddDataLayer(this IServiceCollection services)
    {
        services.ConfigureOptions<DatabaseOptionsSetup>();

        services.AddDbContextPool<MiranaDbContext>((serviceProvider, options) =>
        {
            var databaseOptions = serviceProvider.GetService<IOptions<DatabaseOptions>>()!.Value;
            var hostingEnvironment = serviceProvider.GetService<IHostingEnvironment>();

            options.UseSqlServer(
                databaseOptions.ConnectionString,
                x =>
                {
                    x.MigrationsAssembly(typeof(DependencyInjection).Assembly.FullName);
                    x.EnableRetryOnFailure(databaseOptions.MaxRetryCount);
                    x.CommandTimeout(databaseOptions.CommandTimeout);
                });

            // Check if the environment is development, then enabling detail logging mode
            if (!hostingEnvironment.IsDevelopment()) return;
            options.EnableDetailedErrors(databaseOptions.EnableDetailedErrors);
            options.EnableSensitiveDataLogging(databaseOptions.EnableSensitiveDataLogging);
        });

        return services;
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using miranaSolution.Data.Main;

namespace miranaSolution.Data;

public static class DependencyInjection
{
    public static IServiceCollection AddDataLayer(this IServiceCollection services,
        ConfigurationManager configurationManager)
    {
        services.AddDbContext<MiranaDbContext>(options =>
        {
            options.UseSqlServer(
                configurationManager.GetConnectionString("DefaultConnection"),
                x =>
                {
                    x.MigrationsAssembly(typeof(DependencyInjection).Assembly.FullName);
                });
        });

        return services;
    }
}
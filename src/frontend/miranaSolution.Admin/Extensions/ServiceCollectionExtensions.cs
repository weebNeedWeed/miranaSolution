using miranaSolution.Admin.Refit;
using miranaSolution.Admin.Services.Interfaces;
using Refit;

namespace miranaSolution.Admin.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRefitClients(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var configuration = serviceProvider.GetService<IConfiguration>();
        
        void ConfigureDefaultClient(HttpClient client)
        {
            client.BaseAddress = new Uri(configuration!["BaseAddress"]);
        };

        services.AddTransient<AuthHeaderHandler>();
        
        services.AddRefitClient<IUsersApiService>()
            .ConfigureHttpClient(ConfigureDefaultClient)
            .AddHttpMessageHandler<AuthHeaderHandler>();
        
        services.AddRefitClient<IRolesApiService>()
            .ConfigureHttpClient(ConfigureDefaultClient)
            .AddHttpMessageHandler<AuthHeaderHandler>();
        
        services.AddRefitClient<IBooksApiService>()
            .ConfigureHttpClient(ConfigureDefaultClient)
            .AddHttpMessageHandler<AuthHeaderHandler>();
        
        services.AddRefitClient<ICommentApiService>()
            .ConfigureHttpClient(ConfigureDefaultClient)
            .AddHttpMessageHandler<AuthHeaderHandler>();
        
        services.AddRefitClient<IAuthorsApiService>()
            .ConfigureHttpClient(ConfigureDefaultClient)
            .AddHttpMessageHandler<AuthHeaderHandler>();
        
        services.AddRefitClient<IGenresApiService>()
            .ConfigureHttpClient(ConfigureDefaultClient)
            .AddHttpMessageHandler<AuthHeaderHandler>();
        
        services.AddRefitClient<ISlidesApiService>()
            .ConfigureHttpClient(ConfigureDefaultClient)
            .AddHttpMessageHandler<AuthHeaderHandler>();
        
        services.AddRefitClient<IAuthApiService>()
            .ConfigureHttpClient(ConfigureDefaultClient);

        return services;
    }
}
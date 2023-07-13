using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using miranaSolution.Services.Authentication.Users;
using miranaSolution.Services.Catalog.Authors;
using miranaSolution.Services.Catalog.Books;
using miranaSolution.Services.Catalog.Chapters;
using miranaSolution.Services.Catalog.Genres;
using miranaSolution.Services.Catalog.Slides;
using miranaSolution.Services.Systems.Files;

namespace miranaSolution.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddBusinessLayer(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var configuration = serviceProvider.GetService<IConfiguration>();
        
        // Get Jwt Settings from the configuration file.
        services.Configure<JwtOptions>(configuration!.GetSection(JwtOptions.SectionName));
        
        // Register application's services
        services.AddTransient<ISlideService, SlideService>();
        services.AddTransient<IBookService, BookService>();
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IFileService, FileService>();
        services.AddTransient<IAuthorService, AuthorService>();
        services.AddTransient<IGenreService, GenreService>();
        services.AddTransient<IChapterService, ChapterService>();

        return services;
    }
}
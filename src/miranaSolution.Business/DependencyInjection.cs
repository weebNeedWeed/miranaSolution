using Microsoft.Extensions.DependencyInjection;
using miranaSolution.Business.Auth.Users;
using miranaSolution.Business.Catalog.Authors;
using miranaSolution.Business.Catalog.Books;
using miranaSolution.Business.Catalog.Genres;
using miranaSolution.Business.Systems.Files;
using miranaSolution.Business.Systems.Slides;

namespace miranaSolution.Business;

public static class DependencyInjection
{
    public static IServiceCollection AddBusinessLayer(this IServiceCollection services)
    {
        // Register application's services
        services.AddTransient<ISlideService, SlideService>();
        services.AddTransient<IBookService, BookService>();
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IFileService, FileService>();
        services.AddTransient<IAuthorService, AuthorService>();
        services.AddTransient<IGenreService, GenreService>();

        return services;
    }
}
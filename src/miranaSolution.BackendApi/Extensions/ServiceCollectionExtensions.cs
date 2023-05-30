using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using miranaSolution.Business.Auth.Users;
using miranaSolution.Business.Catalog.Authors;
using miranaSolution.Business.Catalog.Books;
using miranaSolution.Business.Catalog.Genres;
using miranaSolution.Business.Systems.Files;
using miranaSolution.Business.Systems.Slides;
using miranaSolution.Data.Entities;
using miranaSolution.Data.Main;
using miranaSolution.Dtos.Auth.Users.Validations;

namespace miranaSolution.BackendApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBusinessServices(this IServiceCollection services)
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

    public static IServiceCollection AddFluentValidations(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<UserAuthenticationRequestValidator>();

        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services,
        ConfigurationManager configurationManager)
    {
        services.AddDbContext<MiranaDbContext>(options =>
        {
            options.UseSqlServer(configurationManager.GetConnectionString("DefaultConnection"));
        });

        return services;
    }

    public static IServiceCollection AddAndConfigureAuth(this IServiceCollection services,
        ConfigurationManager configurationManager)
    {
        services.AddIdentity<AppUser, AppRole>()
            .AddEntityFrameworkStores<MiranaDbContext>()
            .AddDefaultTokenProviders();

        services.Configure<IdentityOptions>(options =>
        {
            // Password settings.
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 8;
            options.Password.RequiredUniqueChars = 1;

            // SignIn settings
            options.SignIn.RequireConfirmedEmail = false;
            options.SignIn.RequireConfirmedPhoneNumber = false;

            // User settings
            options.User.RequireUniqueEmail = true;
        });

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configurationManager["Jwt:Issuer"],
                ValidAudience = configurationManager["Jwt:Issuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configurationManager["Jwt:Key"])),
                RequireExpirationTime = true,
            };
        });

        return services;
    }

    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "ToDo API",
                Description = "An ASP.NET Core Web API for managing ToDo items",
                //TermsOfService = new Uri("https://example.com/terms"),
            });
        });

        return services;
    }
}
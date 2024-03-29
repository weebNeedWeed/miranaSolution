﻿using Amazon.SimpleEmail;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using miranaSolution.Services.Authentication.PasswordRecovery;
using miranaSolution.Services.Authentication.Roles;
using miranaSolution.Services.Authentication.Users;
using miranaSolution.Services.Core.Authors;
using miranaSolution.Services.Core.Bookmarks;
using miranaSolution.Services.Core.BookRatings;
using miranaSolution.Services.Core.Books;
using miranaSolution.Services.Core.BookUpvotes;
using miranaSolution.Services.Core.Chapters;
using miranaSolution.Services.Core.CommentReactions;
using miranaSolution.Services.Core.Comments;
using miranaSolution.Services.Core.CurrentlyReadings;
using miranaSolution.Services.Core.Genres;
using miranaSolution.Services.Core.Slides;
using miranaSolution.Services.Systems.Files;
using miranaSolution.Services.Systems.Images;
using miranaSolution.Services.Systems.JwtTokenGenerators;
using miranaSolution.Services.Systems.Mails;
using miranaSolution.Services.Validations;
using CurrentlyReading = miranaSolution.Data.Entities.CurrentlyReading;

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
        services.AddTransient<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddTransient<IImageSaver, ImageSaver>();
        services.AddTransient<ICommentService, CommentService>();
        services.AddTransient<ICommentReactionService, CommentReactionService>();
        services.AddTransient<IBookUpvoteService, BookUpvoteService>();
        services.AddTransient<IBookmarkService, BookmarkService>();
        services.AddTransient<IBookRatingService, BookRatingService>();
        services.AddTransient<IMailService, SESService>();
        services.AddTransient<IPasswordRecoveryService, PasswordRecoveryService>();
        services.AddTransient<ICurrentlyReadingService, CurrentlyReadingService>();
        services.AddTransient<IRoleService, RoleService>();

        // AWS mail service's configuration
        services.AddDefaultAWSOptions(configuration.GetAWSOptions());
        services.AddAWSService<IAmazonSimpleEmailService>();
        services.Configure<MailOptions>(configuration.GetSection(MailOptions.SectionName));

        // Add validators from this assembly
        var assembly = typeof(DependencyInjection).Assembly;
        services.AddValidatorsFromAssembly(assembly);

        services.AddTransient<IValidatorProvider, ValidatorProvider>();

        return services;
    }
}
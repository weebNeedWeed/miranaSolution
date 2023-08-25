using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using miranaSolution.Data.Entities;
using miranaSolution.Data.Main;
using miranaSolution.DTOs.Core.Genres;
using miranaSolution.Services.Core.Genres;
using miranaSolution.Services.Exceptions;
using miranaSolution.Services.Validations;
using Moq;
using NuGet.Frameworks;
using Xunit;

namespace miranaSolution.Services.Tests.Core;

public class GenreServiceTests
{
    private readonly GenreService _genreService;
    private readonly MiranaDbContext _context;
    private readonly Mock<IValidatorProvider> _validatorProviderMock = new Mock<IValidatorProvider>();

    public GenreServiceTests()
    {
        var dbContextOptions = new DbContextOptionsBuilder<MiranaDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new MiranaDbContext(dbContextOptions);
        _genreService = new GenreService(_context, _validatorProviderMock.Object);
    }

    [Fact]
    public async Task CreateGenreAsync_ShouldCreateNewGenre_WhenBeingCalled()
    {
        const string name = "some_name";

        var actual = await _genreService.CreateGenreAsync(
            new CreateGenreRequest(name));

        Assert.NotNull(actual.GenreVm);
        Assert.Equal(name, actual.GenreVm.Name);
    }
    
    [Fact]
    public async Task CreateGenreAsync_ShouldThrowValidationException_WhenBeingCalledWithInvalidName()
    {
        _validatorProviderMock.Setup(x => x.Validate(It.IsAny<CreateGenreRequest>()))
            .Throws(new ValidationException(new List<ValidationFailure>()));
        
        await Assert.ThrowsAsync<ValidationException>(
            async () => await _genreService.CreateGenreAsync(
                new CreateGenreRequest("")));
    }
    
    [Fact]
    public async Task GetAllGenresAsync_ShouldReturnAllGenres_WhenBeingCalled()
    {
        var genre1 = new Genre
        {
            Id = 1,
        };
        var genre2 = new Genre
        {
            Id = 2,
        };

        await _context.Genres.AddRangeAsync(new List<Genre> { genre1, genre2 });
        await _context.SaveChangesAsync();

        var actual = await _genreService.GetAllGenresAsync();

        Assert.NotNull(actual.GenreVms);
        Assert.Equal(2, actual.GenreVms.Count);
    }

    [Fact]
    public async Task GetGenreByIdAsync_ShouldReturnAGenre_WhenBeingCalled()
    {
        var genre = new Genre
        {
            Id = 1,
        };
        await _context.Genres.AddAsync(genre);
        await _context.SaveChangesAsync();

        var actual = await _genreService.GetGenreByIdAsync(
            new GetGenreByIdRequest(genre.Id));

        Assert.NotNull(actual.GenreVm);
        Assert.Equal(genre.Id, actual.GenreVm.Id);
    }

    [Fact]
    public async Task GetGenreByIdAsync_ShouldReturnNull_WhenBeingCalledWithInvalidGenreId()
    {
        var actual = await _genreService.GetGenreByIdAsync(
            new GetGenreByIdRequest(1));

        Assert.Null(actual.GenreVm);
    }

    [Fact]
    public async Task DeleteGenreAsync_ShouldDeleteExistingGenre_WhenBeingCalled()
    {
        var genre = new Genre
        {
            Id = 1,
        };
        await _context.Genres.AddAsync(genre);
        await _context.SaveChangesAsync();

        await _genreService.DeleteGenreAsync(
            new DeleteGenreRequest(genre.Id));
        
        Assert.Empty(await _context.Genres.ToListAsync());
    }

    [Fact]
    public async Task DeleteGenreAsync_ShouldThrowGenreNotFoundException_WhenBeingCalledWithInvalidGenreId()
    {
        await Assert.ThrowsAsync<GenreNotFoundException>(
            async () => await _genreService.DeleteGenreAsync(
                new DeleteGenreRequest(1)));
    }

    [Fact]
    public async Task UpdateGenreAsync_ShouldUpdateExistingGenre_WhenBeingCalled()
    {
        var genre = new Genre
        {
            Id = 1,
        };
        await _context.Genres.AddAsync(genre);
        await _context.SaveChangesAsync();
        const string newName = "newName";

        var actual = await _genreService.UpdateGenreAsync(
            new UpdateGenreRequest(genre.Id, newName));

        Assert.NotNull(actual.GenreVm);
        Assert.Equal(newName, actual.GenreVm.Name);
    }

    [Fact]
    public async Task UpdateGenreAsync_ShouldThrowGenreNotFoundException_WhenBeingCalledWithInvalidGenreId()
    {
        await Assert.ThrowsAsync<GenreNotFoundException>(
            async () => await _genreService.UpdateGenreAsync(
                new UpdateGenreRequest(1, "some_name")));
    }

    [Fact]
    public async Task GetAllGenresByBookIdAsync_ShouldReturnAllGenresOfBook_WhenBeingCalled()
    {
        var genre = new Genre
        {
            Id = 1,
        };
        var book = new Book
        {
            Id = 1
        };
        var bookGenre = new BookGenre
        {
            GenreId = genre.Id,
            BookId = book.Id
        };
        await _context.Genres.AddAsync(genre);
        await _context.Books.AddAsync(book);
        await _context.BookGenres.AddAsync(bookGenre);
        await _context.SaveChangesAsync();

        var actual = await _genreService.GetAllGenresByBookIdAsync(
            new GetAllGenresByBookIdRequest(book.Id));

        Assert.NotNull(actual.GenreVms);
        Assert.Single(actual.GenreVms);
    }

    [Fact]
    public async Task GetAllGenresByBookIdAsync_ShouldThrowBookNotFoundException_WhenBeingCalledWithInvalidBookId()
    {
        await Assert.ThrowsAsync<BookNotFoundException>(
            async () => await _genreService.GetAllGenresByBookIdAsync(
                new GetAllGenresByBookIdRequest(1)));
    }
} 
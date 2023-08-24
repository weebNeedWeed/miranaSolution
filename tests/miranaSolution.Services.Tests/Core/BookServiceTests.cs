using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using miranaSolution.Data.Entities;
using miranaSolution.Data.Main;
using miranaSolution.DTOs.Common;
using miranaSolution.DTOs.Core.Books;
using miranaSolution.Services.Core.Books;
using miranaSolution.Services.Exceptions;
using miranaSolution.Services.Systems.Images;
using miranaSolution.Services.Validations;
using Moq;
using Xunit;

namespace miranaSolution.Services.Tests.Core;

public class BookServiceTests
{
    private readonly BookService _bookService;
    private readonly MiranaDbContext _context;
    private readonly Mock<IValidatorProvider> _validatorProviderMock = new();
    private readonly Mock<IImageSaver> _imageSaverMock = new();

    public BookServiceTests()
    {
        var dbContextOptions = new DbContextOptionsBuilder<MiranaDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new MiranaDbContext(dbContextOptions);
        _bookService = new BookService(_context, 
            _imageSaverMock.Object, 
            _validatorProviderMock.Object);
    }

    [Fact]
    public async Task GetBookByIdAsync_ShouldReturnBook_WhenBookWithGivenIdExists()
    {
        var author = new Author
        {
            Id = 1
        };
        var book = new Book
        {
            Id = 1,
            AuthorId = 1
        };
        await _context.Books.AddAsync(book);
        await _context.Authors.AddAsync(author);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        var actual = await _bookService.GetBookByIdAsync(
            new GetBookByIdRequest(book.Id));

        Assert.NotNull(actual.BookVm);
        Assert.Equal(book.Id, actual.BookVm.Id);
        Assert.Equal(book.AuthorId, actual.BookVm.AuthorId);
    }

    [Fact]
    public async Task GetBookByIdAsync_ShouldReturnNull_WhenBookWithGivenIdIsNotFound()
    {
        var actual = await _bookService.GetBookByIdAsync(
            new GetBookByIdRequest(1));
        
        Assert.Null(actual.BookVm);
    }

    [Fact]
    public async Task GetBookBySlugAsync_ShouldReturnBook_WhenBookWithGivenSlugExists()
    {
        var author = new Author
        {
            Id = 1
        };
        var book = new Book
        {
            Id = 1,
            AuthorId = 1,
            Slug = "some_slug"
        };
        await _context.Books.AddAsync(book);
        await _context.Authors.AddAsync(author);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        var actual = await _bookService.GetBookBySlugAsync(
            new GetBookBySlugRequest(book.Slug));

        Assert.NotNull(actual.BookVm);
        Assert.Equal(book.Id, actual.BookVm.Id);
        Assert.Equal(book.AuthorId, actual.BookVm.AuthorId);
        Assert.Equal(book.Slug, actual.BookVm.Slug);
    }
    
    [Fact]
    public async Task GetBookBySlugAsync_ShouldReturnNull_WhenBookWithGivenSlugIsNotFound()
    {
        var actual = await _bookService.GetBookBySlugAsync(
            new GetBookBySlugRequest("some_slug"));
        
        Assert.Null(actual.BookVm);
    }

    [Fact]
    public async Task CreateBookAsync_ShouldCreateNewBook_WhenBeingCalled()
    {
        var author = new Author
        {
            Id = 1
        };

        await _context.Authors.AddAsync(author);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        _imageSaverMock.Setup(
                x => x.SaveImageAsync(
                    It.IsAny<Stream>(),
                    It.IsAny<string>()))
            .ReturnsAsync("some/path");

        const string name = "some_name";
        const string des = "some_des";

        var actual = await _bookService.CreateBookAsync(
            new CreateBookRequest(
                name,
                des,
                des,
                true,
                true,
                "",
                author.Id,
                Stream.Null,
                "ext"));

        Assert.NotNull(actual.BookVm);
        Assert.Equal(name, actual.BookVm.Name);
        Assert.Equal(des, actual.BookVm.LongDescription);
        Assert.Equal(des, actual.BookVm.ShortDescription);
        Assert.Equal(author.Id, actual.BookVm.AuthorId);
        Assert.Equal("some/path", actual.BookVm.ThumbnailImage);
    }

    [Fact]
    public async Task CreateBookAsync_ShouldThrowAuthorNotFoundException_WhenBeingCalledWithInvalidAuthorId()
    {
        await Assert.ThrowsAsync<AuthorNotFoundException>(
            async () => await _bookService.CreateBookAsync(
                new CreateBookRequest(
                    "name",
                    "des",
                    "des",
                    true,
                    true,
                    "",
                    1,
                    Stream.Null,
                    "ext")));
    }

    [Fact]
    public async Task CreateBookAsync_ShouldThrowBookAlreadyExistsException_WhenBeingCalledWithExistingSlug()
    {
        var author = new Author
        {
            Id = 1
        };
        var book = new Book
        {
            Id = 1,
            AuthorId = 1,
            Slug = "some_slug"
        };
        await _context.Books.AddAsync(book);
        await _context.Authors.AddAsync(author);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        await Assert.ThrowsAsync<BookAlreadyExistsException>(
            async () => await _bookService.CreateBookAsync(
                new CreateBookRequest(
                    "name",
                    "des",
                    "des",
                    true,
                    true,
                    book.Slug,
                    1,
                    Stream.Null,
                    "ext")));
    }

    [Fact]
    public async Task CreateBookAsync_ShouldThrowValidationException_WhenBeingCalledWithInvalidParameters()
    {
        _validatorProviderMock.Setup(x => x.Validate(It.IsAny<CreateBookRequest>()))
            .Throws(new ValidationException(new List<ValidationFailure>()));
        
        await Assert.ThrowsAsync<ValidationException>(
            async () => await _bookService.CreateBookAsync(
                new CreateBookRequest(
                    "name",
                    "des",
                    "des",
                    true,
                    true,
                    "",
                    1,
                    Stream.Null,
                    "ext")));
    }

    [Fact]
    public async Task UpdateBookAsync_ShouldUpdateExistingBook_WhenBeingCalled()
    {
        var author = new Author
        {
            Id = 1
        };
        var book = new Book
        {
            Id = 1,
            AuthorId = 1,
            Slug = "some_slug",
            ThumbnailImage = "some/old/path"
        };
        await _context.Books.AddAsync(book);
        await _context.Authors.AddAsync(author);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();
        
        _imageSaverMock.Setup(
                x => x.SaveImageAsync(
                    It.IsAny<Stream>(),
                    It.IsAny<string>()))
            .ReturnsAsync("some/path");

        const string newName = "newName";
        const string newDes = "newDes";

        var actual = await _bookService.UpdateBookAsync(
            new UpdateBookRequest(
                book.Id,
                newName,
                newDes,
                newDes,
                true,
                book.Slug,
                book.AuthorId,
                true,
                Stream.Null,
                "ext"));

        Assert.NotNull(actual.BookVm);
        Assert.Equal(newName, actual.BookVm.Name);
        Assert.Equal(newDes, actual.BookVm.LongDescription);
        Assert.Equal(newDes, actual.BookVm.ShortDescription);
        Assert.Equal("some/path", actual.BookVm.ThumbnailImage);
    }

    [Fact]
    public async Task UpdateBookAsync_ShouldThrowAuthorNotFoundException_WhenBeingCalledWithInvalidAuthorId()
    {
        var author = new Author
        {
            Id = 1
        };
        var book = new Book
        {
            Id = 1,
            AuthorId = 1,
            Slug = "some_slug"
        };
        await _context.Books.AddAsync(book);
        await _context.Authors.AddAsync(author);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();
        
        await Assert.ThrowsAsync<AuthorNotFoundException>(
            async () => await _bookService.UpdateBookAsync(
                new UpdateBookRequest(
                    book.Id,
                    "name",
                    "des",
                    "des",
                    true,
                    "true",
                    2,
                    true,
                    Stream.Null,
                    "ext")));
    }

    [Fact]
    public async Task UpdateBookAsync_ShouldThrowBookAlreadyExistsException_WhenBeingCalledWithExistingSlug()
    {
        var author = new Author
        {
            Id = 1
        };
        var book1 = new Book
        {
            Id = 1,
            AuthorId = 1,
            Slug = "some_slug"
        };
        var book2 = new Book
        {
            Id = 2,
            AuthorId = 1,
            Slug = "some_slug_2"
        };
        await _context.Books.AddRangeAsync(new List<Book>{book1, book2});
        await _context.Authors.AddAsync(author);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();
        
        await Assert.ThrowsAsync<BookAlreadyExistsException>(
            async () => await _bookService.UpdateBookAsync(
                new UpdateBookRequest(
                    book1.Id,
                    "name",
                    "des",
                    "des",
                    true,
                    book2.Slug,
                    1,
                    true,
                    Stream.Null,
                    "ext")));
    }

    [Fact]
    public async Task UpdateBookAsync_ShouldThrowBookNotFoundException_WhenBeingCalledWithInvalidBookId()
    {
        await Assert.ThrowsAsync<BookNotFoundException>(
            async () => await _bookService.UpdateBookAsync(
                new UpdateBookRequest(
                    1,
                    "name",
                    "des",
                    "des",
                    true,
                    "",
                    1,
                    true,
                    Stream.Null,
                    "ext")));
    }

    [Fact]
    public async Task UpdateBookAsync_ShouldThrowValidationException_WhenBeingCalledWithInvalidParameters()
    {
        _validatorProviderMock.Setup(x => x.Validate(It.IsAny<UpdateBookRequest>()))
            .Throws(new ValidationException(new List<ValidationFailure>()));
        
        await Assert.ThrowsAsync<ValidationException>(
            async () => await _bookService.UpdateBookAsync(
                new UpdateBookRequest(
                    1,
                    "name",
                    "des",
                    "des",
                    true,
                    "",
                    1,
                    true,
                    Stream.Null,
                    "ext")));
    }

    [Fact]
    public async Task DeleteBookAsync_ShouldDeleteExistingBook_WhenBeingCalled()
    {
        var book = new Book
        {
            Id = 1,
        };
        await _context.Books.AddAsync(book);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        await _bookService.DeleteBookAsync(
            new DeleteBookRequest(book.Id));
        
        Assert.Empty(await _context.Books.ToListAsync());
    }

    [Fact]
    public async Task DeleteBookAsync_ShouldThrowBookNotFoundException_WhenBeingCalledWithInvalidBookId()
    {
        await Assert.ThrowsAsync<BookNotFoundException>(
            async () => await _bookService.DeleteBookAsync(
                new DeleteBookRequest(1)));
    }

    [Fact]
    public async Task GetRecommendedBooksAsync_ShouldReturnRecommendedBooks_WhenBeingCalled()
    {
        var author = new Author
        {
            Id = 1
        };
        var book1 = new Book
        {
            Id = 1,
            AuthorId = 1,
            Slug = "some_slug",
            IsRecommended = true
        };
        var book2 = new Book
        {
            Id = 2,
            AuthorId = 1,
            Slug = "some_slug_2",
            IsRecommended = true
        };
        await _context.Books.AddRangeAsync(new List<Book>{book1, book2});
        await _context.Authors.AddAsync(author);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        var actual = await _bookService.GetRecommendedBooksAsync();

        Assert.NotNull(actual.BookVms);
        Assert.Equal(2, actual.BookVms.Count);
    }

    [Fact]
    public async Task GetAllBooksAsync_ShouldReturnBooks_WhenBeingCalled()
    {
        var author1 = new Author
        {
            Id = 1
        };
        var author2 = new Author
        {
            Id = 2
        };
        
        var genre = new Genre
        {
            Id = 1,
            Name = "genre"
        };
        
        var book1 = new Book
        {
            Id = 1,
            Name = "some_name",
            AuthorId = 1,
            Slug = "some_slug",
            IsDone = true
        };
        var book2 = new Book
        {
            Id = 2,
            AuthorId = 2,
            Slug = "some_slug_2",
            IsDone = false
        };

        var bookGenre = new BookGenre
        {
            BookId = book1.Id,
            GenreId = genre.Id
        };

        await _context.Genres.AddAsync(genre);
        await _context.Books.AddRangeAsync(new List<Book>{book2, book1});
        await _context.BookGenres.AddAsync(bookGenre);
        await _context.Authors.AddRangeAsync(new List<Author> {author1, author2});
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();
        
        var actual = await _bookService.GetAllBooksAsync(
            new GetAllBooksRequest(null, null, null, null, new PagerRequest(1, 10)));

        Assert.NotNull(actual.BookVms);
        Assert.Equal(2, actual.BookVms.Count);
        
        var actual2 = await _bookService.GetAllBooksAsync(
            new GetAllBooksRequest(book1.Name, null, null, null, new PagerRequest(1, 10)));
        
        Assert.NotNull(actual2.BookVms);
        Assert.Single(actual2.BookVms);
        Assert.Equal(book1.Name, actual2.BookVms[0].Name);
        
        var actual3 = await _bookService.GetAllBooksAsync(
            new GetAllBooksRequest(null, genre.Id.ToString(), null, null, new PagerRequest(1, 10)));
        
        Assert.NotNull(actual3.BookVms);
        Assert.Single(actual3.BookVms);
        Assert.Equal(genre.Name, actual3.BookVms[0].Genres[0]);
        
        var actual4 = await _bookService.GetAllBooksAsync(
            new GetAllBooksRequest(null, null, true, null, new PagerRequest(1, 10)));
        
        Assert.NotNull(actual4.BookVms);
        Assert.Single(actual4.BookVms);
        Assert.True(actual4.BookVms[0].IsDone);
        
        var actual5 = await _bookService.GetAllBooksAsync(
            new GetAllBooksRequest(null, null, null, author2.Id, new PagerRequest(1, 10)));
        
        Assert.NotNull(actual5.BookVms);
        Assert.Single(actual5.BookVms);
        Assert.Equal(author2.Id, actual5.BookVms[0].AuthorId);
        
        Assert.Equal(1, actual5.PagerResponse.PageIndex);
        Assert.Equal(10, actual5.PagerResponse.PageSize);
    }

    [Fact]
    public async Task GetMostReadingBooks_ShouldReturnMostReadingBooks_WhenBeingCalled()
    {
        var author = new Author
        {
            Id = 1
        };
        var book1 = new Book
        {
            Id = 1,
            AuthorId = 1,
            ViewCount = 1
        };
        var book2 = new Book
        {
            Id = 2,
            AuthorId = 1,
            ViewCount = 10
        };
        await _context.Books.AddRangeAsync(new List<Book>{book1, book2});
        await _context.Authors.AddAsync(author);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        var actual = await _bookService.GetMostReadingBooks(
            new GetMostReadingBooksRequest(10));

        Assert.NotNull(actual.BookVms);
        Assert.Equal(2, actual.BookVms.Count);
        Assert.Collection(actual.BookVms,
            x => Assert.Equal(book2.ViewCount, x.ViewCount),
            x => Assert.Equal(book1.ViewCount, x.ViewCount));
    }

    [Fact]
    public async Task AssignGenresAsync_ShouldAssignGenreToBook_WhenBeingCalled()
    {
        var book = new Book
        {
            Id = 1,
        };

        var genre1 = new Genre
        {
            Id = 1,
            Name = "genre1"
        };
        
        var genre2 = new Genre
        {
            Id = 2,
            Name = "genre2"
        };
        
        var genre3 = new Genre
        {
            Id = 3,
            Name = "genre3"
        };

        var bookGenre = new BookGenre
        {
            BookId = book.Id,
            GenreId = genre3.Id
        };
        
        await _context.Books.AddAsync(book);
        await _context.Genres.AddRangeAsync(new List<Genre>{genre1, genre2, genre3});
        await _context.BookGenres.AddAsync(bookGenre);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        var checkboxItemList = new List<CheckboxItem>()
        {
            new CheckboxItem
            {
                Id = genre1.Id,
                Label = genre1.Name,
                IsChecked = true
            },
            new CheckboxItem
            {
                Id = genre2.Id,
                Label = genre2.Name,
                IsChecked = true
            },
            new CheckboxItem
            {
                Id = genre3.Id,
                Label = genre3.Name,
                IsChecked = false
            },
        };
        
        await _bookService.AssignGenresAsync(
            new AssignGenresRequest(
                book.Id, checkboxItemList));
        
        Assert.Equal(2, await _context.BookGenres.CountAsync());
        Assert.Collection(
            await _context.BookGenres.ToListAsync(),
            x => Assert.Equal(genre1.Id, x.GenreId),
            x => Assert.Equal(genre2.Id, x.GenreId));
    }

    [Fact]
    public async Task AssignGenresAsync_ShouldThrowBookNotFoundException_WhenBeingCalledWithInvalidBookId()
    {
        await Assert.ThrowsAsync<BookNotFoundException>(
            async () => await _bookService.AssignGenresAsync(
                new AssignGenresRequest(1, new List<CheckboxItem>())));
    }

    [Fact]
    public async Task AssignGenresAsync_ShouldThrowValidationException_WhenBeingCalledWithInvalidParameters()
    {
        _validatorProviderMock.Setup(x => x.Validate(It.IsAny<AssignGenresRequest>()))
            .Throws(new ValidationException(new List<ValidationFailure>()));
        
        await Assert.ThrowsAsync<ValidationException>(
            async () => await _bookService.AssignGenresAsync(
                new AssignGenresRequest(1, new List<CheckboxItem>())));
    }
}
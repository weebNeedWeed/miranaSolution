using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using miranaSolution.Data.Entities;
using miranaSolution.Data.Main;
using miranaSolution.DTOs.Core.Bookmarks;
using miranaSolution.DTOs.Core.BookUpvotes;
using miranaSolution.Services.Core.BookUpvotes;
using miranaSolution.Services.Exceptions;
using Moq;
using Xunit;

namespace miranaSolution.Services.Tests.Core;

public class BookUpvoteServiceTests
{
    private readonly BookUpvoteService _bookUpvoteService;
    private readonly MiranaDbContext _context;

    private readonly Mock<UserManager<AppUser>> _userManagerMock = new(
        Mock.Of<IUserStore<AppUser>>(),
        null!, null!, null!, null!,
        null!, null!, null!, null!);

    public BookUpvoteServiceTests()
    {
        var dbContextOptions = new DbContextOptionsBuilder<MiranaDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new MiranaDbContext(dbContextOptions);
        _bookUpvoteService = new BookUpvoteService(_context, _userManagerMock.Object);
    }

    [Fact]
    public async Task CreateBookUpvoteAsync_ShouldCreateNewUpvote_WhenBeingCalled()
    {
        var user = new AppUser
        {
            Id = Guid.NewGuid(),
        };
        var book = new Book
        {
            Id = 1,
        };

        _userManagerMock
            .Setup(x => x.FindByIdAsync(user.Id.ToString()))
            .ReturnsAsync(user);

        await _context.Books.AddAsync(book);
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        var actual = await _bookUpvoteService.CreateBookUpvoteAsync(
            new CreateBookUpvoteRequest(user.Id, book.Id));

        Assert.NotNull(actual.BookUpvoteVm);
        Assert.Equal(user.Id, actual.BookUpvoteVm.UserId);
        Assert.Equal(book.Id, actual.BookUpvoteVm.BookId);
    }

    [Fact]
    public async Task CreateBookUpvoteAsync_ShouldThrowUserNotFoundException_WhenBeingCalledWithInvalidUserId()
    {
        _userManagerMock
            .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(() => null!);

        await Assert.ThrowsAsync<UserNotFoundException>(
            async () => await _bookUpvoteService.CreateBookUpvoteAsync(
                new CreateBookUpvoteRequest(Guid.NewGuid(), 1)));
    }

    [Fact]
    public async Task CreateBookUpvoteAsync_ShouldThrowBookNotFoundException_WhenBeingCalledWithInvalidBookId()
    {
        var userId = Guid.NewGuid();
        var user = new AppUser
        {
            Id = userId,
        };
        _userManagerMock
            .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(user);

        await Assert.ThrowsAsync<BookNotFoundException>(
            async () => await _bookUpvoteService.CreateBookUpvoteAsync(
                new CreateBookUpvoteRequest(user.Id, 1)));
    }

    [Fact]
    public async Task CreateBookUpvoteAsync_ShouldThrowUserAlreadyUpvotesBookException_WhenUserAlreadyUpvotes()
    {
        var user = new AppUser
        {
            Id = Guid.NewGuid(),
        };
        var book = new Book
        {
            Id = 1,
        };
        var bookUpvote = new BookUpvote
        {
            UserId = user.Id,
            BookId = book.Id
        };

        await _context.Books.AddAsync(book);
        await _context.Users.AddAsync(user);
        await _context.BookUpvotes.AddAsync(bookUpvote);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        _userManagerMock
            .Setup(x => x.FindByIdAsync(user.Id.ToString()))
            .ReturnsAsync(user);

        await Assert.ThrowsAsync<UserAlreadyUpvotesBookException>(
            async () => await _bookUpvoteService.CreateBookUpvoteAsync(
                new CreateBookUpvoteRequest(user.Id, book.Id)));
    }

    [Fact]
    public async Task DeleteBookUpvoteAsync_ShouldUnUpvoteBook_WhenBeingCalled()
    {
        var user = new AppUser
        {
            Id = Guid.NewGuid(),
        };
        var book = new Book
        {
            Id = 1,
        };
        var bookUpvote = new BookUpvote
        {
            UserId = user.Id,
            BookId = book.Id
        };

        await _context.Books.AddAsync(book);
        await _context.Users.AddAsync(user);
        await _context.BookUpvotes.AddAsync(bookUpvote);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        _userManagerMock
            .Setup(x => x.FindByIdAsync(user.Id.ToString()))
            .ReturnsAsync(user);

        await _bookUpvoteService.DeleteBookUpvoteAsync(
            new DeleteBookUpvoteRequest(user.Id, book.Id));

        Assert.Empty(await _context.BookUpvotes.ToListAsync());
    }

    [Fact]
    public async Task DeleteBookUpvoteAsync_ShouldThrowBookNotFoundException_WhenBeingCalledWithInvalidBookId()
    {
        var userId = Guid.NewGuid();
        var user = new AppUser
        {
            Id = userId,
        };
        _userManagerMock
            .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(user);

        await Assert.ThrowsAsync<BookNotFoundException>(
            async () => await _bookUpvoteService.DeleteBookUpvoteAsync(
                new DeleteBookUpvoteRequest(user.Id, 1)));
    }

    [Fact]
    public async Task DeleteBookUpvoteAsync_ShouldThrowUserNotFoundException_WhenBeingCalledWithInvalidUserId()
    {
        _userManagerMock
            .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(() => null!);

        await Assert.ThrowsAsync<UserNotFoundException>(
            async () => await _bookUpvoteService.DeleteBookUpvoteAsync(
                new DeleteBookUpvoteRequest(Guid.NewGuid(), 1)));
    }
    
    [Fact]
    public async Task DeleteBookUpvoteAsync_ShouldThrowUserNotUpvotedBookException_WhenUserDidNotUpvoteBook()
    {
        var user = new AppUser
        {
            Id = Guid.NewGuid(),
        };
        var book = new Book
        {
            Id = 1,
        };

        _userManagerMock
            .Setup(x => x.FindByIdAsync(user.Id.ToString()))
            .ReturnsAsync(user);

        await _context.Books.AddAsync(book);
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        await Assert.ThrowsAsync<UserNotUpvotedBookException>(
            async () => await _bookUpvoteService.DeleteBookUpvoteAsync(
                new DeleteBookUpvoteRequest(user.Id, book.Id)));
    }

    [Fact]
    public async Task CountBookUpvoteByUserIdAsync_ShouldReturnNumberOfUpvotes_WhenBeingCalled()
    {
        var user = new AppUser
        {
            Id = Guid.NewGuid(),
        };
        var book = new Book
        {
            Id = 1,
        };
        var bookUpvote = new BookUpvote
        {
            UserId = user.Id,
            BookId = book.Id
        };

        _userManagerMock
            .Setup(x => x.FindByIdAsync(user.Id.ToString()))
            .ReturnsAsync(user);
        
        await _context.Books.AddAsync(book);
        await _context.Users.AddAsync(user);
        await _context.BookUpvotes.AddAsync(bookUpvote);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        var actual = await _bookUpvoteService.CountBookUpvoteByUserIdAsync(
            new CountBookUpvoteByUserIdRequest(user.Id));
        
        Assert.Equal(1, actual.TotalUpvotes);
    }

    [Fact]
    public async Task CountBookUpvoteByUserIdAsync_ShouldThrowUserNotFoundException_WhenBeingCalledWithInvalidUserId()
    {
        _userManagerMock
            .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(() => null!);

        await Assert.ThrowsAsync<UserNotFoundException>(
            async () => await _bookUpvoteService.CountBookUpvoteByUserIdAsync(
                new CountBookUpvoteByUserIdRequest(Guid.NewGuid())));
    }

    [Fact]
    public async Task CountBookUpvoteByBookIdAsync_ShouldThrowBookNotFoundException_WhenBeingCalledWithInvalidBookId()
    {
        await Assert.ThrowsAsync<BookNotFoundException>(
            async () => await _bookUpvoteService.CountBookUpvoteByBookIdAsync(
                new CountBookUpvoteByBookIdRequest(1)));
    }

    [Fact]
    public async Task CountBookUpvoteByBookIdAsync_ShouldReturnNumberOfUpvotes_WhenBeingCalled()
    {
        var user = new AppUser
        {
            Id = Guid.NewGuid(),
        };
        var book = new Book
        {
            Id = 1,
        };
        var bookUpvote = new BookUpvote
        {
            UserId = user.Id,
            BookId = book.Id
        };

        await _context.Books.AddAsync(book);
        await _context.Users.AddAsync(user);
        await _context.BookUpvotes.AddAsync(bookUpvote);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        var actual = await _bookUpvoteService.CountBookUpvoteByBookIdAsync(
            new CountBookUpvoteByBookIdRequest(book.Id));
        
        Assert.Equal(1, actual.TotalUpvotes);
    }

    [Fact]
    public async Task GetAllBookUpvotesAsync_ShouldReturnAllUpvotes_WhenBeingCalled()
    {
        var user = new AppUser
        {
            Id = Guid.NewGuid(),
        };
        var book = new Book
        {
            Id = 1,
        };
        var bookUpvote = new BookUpvote
        {
            UserId = user.Id,
            BookId = book.Id
        };

        await _context.Books.AddAsync(book);
        await _context.Users.AddAsync(user);
        await _context.BookUpvotes.AddAsync(bookUpvote);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        var actual = await _bookUpvoteService.GetAllBookUpvotesAsync(
            new GetAllBookUpvotesRequest(book.Id, null));
        
        Assert.Single(actual.BookUpvoteVms);
        
        var actual2 = await _bookUpvoteService.GetAllBookUpvotesAsync(
            new GetAllBookUpvotesRequest(book.Id, user.Id));
        
        Assert.Single(actual2.BookUpvoteVms);
        
        var actual3 = await _bookUpvoteService.GetAllBookUpvotesAsync(
            new GetAllBookUpvotesRequest(book.Id, Guid.NewGuid()));
        
        Assert.Empty(actual3.BookUpvoteVms);
    }

    [Fact]
    public async Task GetAllBookUpvotesAsync_ShouldThrowBookNotFoundException_WhenBeingCalledWithInvalidBookId()
    {
        await Assert.ThrowsAsync<BookNotFoundException>(
            async () => await _bookUpvoteService.GetAllBookUpvotesAsync(
                new GetAllBookUpvotesRequest(1,null)));
    }
}
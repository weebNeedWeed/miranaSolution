using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using miranaSolution.Data.Entities;
using miranaSolution.Data.Main;
using miranaSolution.DTOs.Common;
using miranaSolution.DTOs.Core.BookRatings;
using miranaSolution.DTOs.Core.Books;
using miranaSolution.Services.Core.BookRatings;
using miranaSolution.Services.Exceptions;
using miranaSolution.Services.Validations;
using Moq;
using Xunit;

namespace miranaSolution.Services.Tests.Core;

public class BookRatingServiceTests
{
    private readonly BookRatingService _bookRatingService;
    private readonly MiranaDbContext _context;
    private readonly Mock<IValidatorProvider> _validatorProviderMock = new();
    private readonly Mock<UserManager<AppUser>> _userManagerMock = new(
        Mock.Of<IUserStore<AppUser>>(),
        null!,null!,null!,null!,
        null!,null!,null!,null!);
    
    public BookRatingServiceTests()
    {
        var dbContextOptions = new DbContextOptionsBuilder<MiranaDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new MiranaDbContext(dbContextOptions);

        _bookRatingService = new BookRatingService(_context, 
            _userManagerMock.Object, _validatorProviderMock.Object);
    }

    [Fact]
    public async Task CreateBookRatingAsync_ShouldCreateNewRating_WhenBeingCalled()
    {
        var user = new AppUser
        {
            Id = Guid.NewGuid(),
        };
        var book = new Book
        {
            Id = 1,
        };
        
        await _context.Books.AddAsync(book);
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();
        
        _userManagerMock
            .Setup(x => x.FindByIdAsync(user.Id.ToString()))
            .ReturnsAsync(user);

        var content = "some_content";
        var star = 5;
        
        var actual = await _bookRatingService.CreateBookRatingAsync(
            new CreateBookRatingRequest(user.Id, book.Id, content, star));

        Assert.NotNull(actual.BookRatingVm);
        Assert.Equal(user.Id, actual.BookRatingVm.UserId);
        Assert.Equal(book.Id, actual.BookRatingVm.BookId);
        Assert.Equal(content, actual.BookRatingVm.Content);
        Assert.Equal(star, actual.BookRatingVm.Star);
    }

    [Fact]
    public async Task CreateBookRatingAsync_ShouldThrowUserNotFoundException_WhenBeingCalledWithInvalidUserId()
    {
        _userManagerMock
            .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(() => null!);

        await Assert.ThrowsAsync<UserNotFoundException>(
            async () => await _bookRatingService.CreateBookRatingAsync(
                new (Guid.NewGuid(), 1, "SomeContent", 5)));
    }
    
    [Fact]
    public async Task CreateBookRatingAsync_ShouldThrowBookNotFoundException_WhenBeingCalledWithInvalidBookId()
    {
        _userManagerMock
            .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(new AppUser());

        await Assert.ThrowsAsync<BookNotFoundException>(
            async () => await _bookRatingService.CreateBookRatingAsync(
                new (Guid.NewGuid(), 1, "SomeContent", 5)));
    }

    [Fact]
    public async Task CreateBookRatingAsync_ShouldThrowBookRatingAlreadyExistsException_WhenBookRatingBeingAlreadyAdded()
    {
        var user = new AppUser
        {
            Id = Guid.NewGuid(),
        };
        var book = new Book
        {
            Id = 1,
        };
        var bookRating = new BookRating
        {
            BookId = book.Id,
            UserId = user.Id,
        };

        await _context.Books.AddAsync(book);
        await _context.Users.AddAsync(user);
        await _context.BookRatings.AddAsync(bookRating);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();
        
        _userManagerMock
            .Setup(x => x.FindByIdAsync(user.Id.ToString()))
            .ReturnsAsync(user);

        await Assert.ThrowsAsync<BookRatingAlreadyExistsException>(
            async () => await _bookRatingService.CreateBookRatingAsync(
                new CreateBookRatingRequest(user.Id, book.Id, "some_content", 5)));
    }

    [Fact]
    public async Task
        CreateBookRatingAsync_ShouldThrowValidationException_WhenBeingCalledWithInvalidParameters()
    {
        _validatorProviderMock.Setup(x => x.Validate(It.IsAny<CreateBookRatingRequest>()))
            .Throws(() => new ValidationException(new List<ValidationFailure>()));
        
        await Assert.ThrowsAsync<ValidationException>(
            async () => await _bookRatingService.CreateBookRatingAsync(
                new CreateBookRatingRequest(Guid.NewGuid(), 1, "", 0)));
    }

    [Fact]
    public async Task DeleteBookRatingAsync_ShouldDeleteExistingRating_WhenBeingCalled()
    {
        var user = new AppUser
        {
            Id = Guid.NewGuid(),
        };
        var book = new Book
        {
            Id = 1,
        };
        var bookRating = new BookRating
        {
            BookId = book.Id,
            UserId = user.Id,
        };

        await _context.Books.AddAsync(book);
        await _context.Users.AddAsync(user);
        await _context.BookRatings.AddAsync(bookRating);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();
        
        _userManagerMock
            .Setup(x => x.FindByIdAsync(user.Id.ToString()))
            .ReturnsAsync(user);

        await _bookRatingService.DeleteBookRatingAsync(
            new DeleteBookRatingRequest(user.Id, book.Id));
        
        Assert.Empty(_context.BookRatings);
    }

    [Fact]
    public async Task DeleteBookRatingAsync_ShouldThrowBookRatingNotFoundException_WhenBookRatingIsNotAdded()
    {
        await Assert.ThrowsAsync<BookRatingNotFoundException>(
            async () => await _bookRatingService.DeleteBookRatingAsync(
                new DeleteBookRatingRequest(Guid.NewGuid(), 1)));
    }

    [Fact]
    public async Task UpdateBookRatingAsync_ShouldUpdateExistingRating_WhenBeingCalled()
    {
        var user = new AppUser
        {
            Id = Guid.NewGuid(),
        };
        var book = new Book
        {
            Id = 1,
        };
        var bookRating = new BookRating
        {
            BookId = book.Id,
            UserId = user.Id,
            Content = "content",
            Star = 5
        };

        await _context.Books.AddAsync(book);
        await _context.Users.AddAsync(user);
        await _context.BookRatings.AddAsync(bookRating);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();
        
        _userManagerMock
            .Setup(x => x.FindByIdAsync(user.Id.ToString()))
            .ReturnsAsync(user);

        var newContent = "new content";
        var newStar = 4;

        var actual = await _bookRatingService.UpdateBookRatingAsync(
            new UpdateBookRatingRequest(user.Id, book.Id, newContent, newStar));

        Assert.NotNull(actual.BookRatingVm);
        Assert.Equal(newContent, actual.BookRatingVm.Content);
        Assert.Equal(newStar, actual.BookRatingVm.Star);
    }

    [Fact]
    public async Task UpdateBookRatingAsync_ShouldThrowBookRatingNotFoundException_WhenBookRatingIsNotAdded()
    {
        await Assert.ThrowsAsync<BookRatingNotFoundException>(
            async () => await _bookRatingService.UpdateBookRatingAsync(
                new UpdateBookRatingRequest(Guid.NewGuid(), 1, "", 1)));
    }

    [Fact]
    public async Task GetAllBookRatingsByUserIdAsync_ShouldReturnAllBookRatingsOfUser_WhenBeingCalled()
    {
        var user = new AppUser
        {
            Id = Guid.NewGuid(),
        };
        var book = new Book
        {
            Id = 1,
        };
        var bookRating = new BookRating
        {
            BookId = book.Id,
            UserId = user.Id,
        };

        await _context.Books.AddAsync(book);
        await _context.Users.AddAsync(user);
        await _context.BookRatings.AddAsync(bookRating);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();
        
        _userManagerMock
            .Setup(x => x.FindByIdAsync(user.Id.ToString()))
            .ReturnsAsync(user);

        var actual = await _bookRatingService.GetAllBookRatingsByUserIdAsync(
            new GetAllBookRatingsByUserIdRequest(user.Id));

        Assert.NotNull(actual.BookRatingVms);
        Assert.Single(actual.BookRatingVms);
    }
    
    [Fact]
    public async Task GetAllBookRatingsByUserIdAsync_ShouldThrowUserNotFoundException_WhenBeingCalledWithInvalidUserId()
    {
        _userManagerMock
            .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(() => null!);

        await Assert.ThrowsAsync<UserNotFoundException>(
            async () => await _bookRatingService.GetAllBookRatingsByUserIdAsync(
                new(Guid.NewGuid())));
    }
    
    [Fact]
    public async Task GetAllBookRatingsByBookIdAsync_ShouldReturnAllBookRatingsOfUser_WhenBeingCalled()
    {
        var user = new AppUser
        {
            Id = Guid.NewGuid(),
        };
        var book = new Book
        {
            Id = 1,
        };
        var bookRating = new BookRating
        {
            BookId = book.Id,
            UserId = user.Id,
        };

        await _context.Books.AddAsync(book);
        await _context.Users.AddAsync(user);
        await _context.BookRatings.AddAsync(bookRating);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();
        
        _userManagerMock
            .Setup(x => x.FindByIdAsync(user.Id.ToString()))
            .ReturnsAsync(user);

        var actual1 = await _bookRatingService.GetAllBookRatingsByBookIdAsync(
            new GetAllBookRatingsByBookIdRequest(book.Id, null, new PagerRequest(1, 10)));

        Assert.NotNull(actual1.BookRatingVms);
        Assert.Single(actual1.BookRatingVms);

        var actual2 = await _bookRatingService.GetAllBookRatingsByBookIdAsync(
            new GetAllBookRatingsByBookIdRequest(book.Id, user.Id, new PagerRequest(1, 10)));

        Assert.NotNull(actual2.BookRatingVms);
        Assert.Single(actual2.BookRatingVms);
        
        var actual3 = await _bookRatingService.GetAllBookRatingsByBookIdAsync(
            new GetAllBookRatingsByBookIdRequest(book.Id, Guid.NewGuid(), new PagerRequest(1, 10)));
        
        Assert.NotNull(actual3.BookRatingVms);
        Assert.Empty(actual3.BookRatingVms);
        Assert.Equal(1, actual3.PagerResponse.PageIndex);
        Assert.Equal(10, actual3.PagerResponse.PageSize);
    }

    [Fact]
    public async Task GetAllBookRatingsByBookIdAsync_ShouldThrowBookNotFoundException_WhenBeingCalledWithInvalidBookId()
    {
        _userManagerMock
            .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(new AppUser());

        await Assert.ThrowsAsync<BookNotFoundException>(
            async () => await _bookRatingService.GetAllBookRatingsByBookIdAsync(
                new (1, null, new PagerRequest(1, 10))));
    }

    [Fact]
    public async Task CheckUserIsRatedAsync_ShouldReturnTrue_WhenUserIsRated()
    {
        var user = new AppUser
        {
            Id = Guid.NewGuid(),
        };
        var book = new Book
        {
            Id = 1,
        };
        var bookRating = new BookRating
        {
            BookId = book.Id,
            UserId = user.Id,
        };

        await _context.Books.AddAsync(book);
        await _context.Users.AddAsync(user);
        await _context.BookRatings.AddAsync(bookRating);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        var actual = await _bookRatingService.CheckUserIsRatedAsync(
            new CheckUserIsRatedRequest(user.Id, book.Id));
        
        Assert.True(actual.IsRated);
    }
    
    [Fact]
    public async Task CheckUserIsRatedAsync_ShouldReturnFalse_WhenUserIsNotRated()
    {
        var actual = await _bookRatingService.CheckUserIsRatedAsync(
            new CheckUserIsRatedRequest(Guid.NewGuid(), 1));
        
        Assert.False(actual.IsRated);
    }

    [Fact]
    public async Task GetOverviewAsync_ShouldReturnOverviewOfRatingStars_WhenBeingCalled()
    {
        var user1 = new AppUser
        {
            Id = Guid.NewGuid(),
        };
        var user2 = new AppUser
        {
            Id = Guid.NewGuid(),
        };
        var book = new Book
        {
            Id = 1,
        };
        var bookRating1 = new BookRating
        {
            BookId = book.Id,
            UserId = user1.Id,
            Star = 1
        };
        var bookRating2 = new BookRating
        {
            BookId = book.Id,
            UserId = user2.Id,
            Star = 2
        };

        await _context.Books.AddAsync(book);
        await _context.Users.AddRangeAsync(new List<AppUser> {user1, user2});
        await _context.BookRatings.AddRangeAsync(new List<BookRating> {bookRating1, bookRating2});
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        var actual = await _bookRatingService.GetOverviewAsync(
            new GetOverviewRequest(book.Id));

        Assert.NotNull(actual.RatingsByStar);
        Assert.Equal(2, actual.TotalRatings);
        Assert.Equal(2, actual.Avg);
        Assert.Collection(actual.RatingsByStar, 
            x => Assert.Equal(1, x.Value),
            x => Assert.Equal(1, x.Value),
            x => Assert.Equal(0, x.Value),
            x => Assert.Equal(0, x.Value),
            x => Assert.Equal(0, x.Value));
    }
    
    [Fact]
    public async Task GetOverviewAsync_ShouldThrowBookNotFoundException_WhenBeingCalledWithInvalidBookId()
    {
        await Assert.ThrowsAsync<BookNotFoundException>(
            async () => await _bookRatingService.GetOverviewAsync(
                new (1)));
    }
}
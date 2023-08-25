using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using miranaSolution.Data.Entities;
using miranaSolution.Data.Main;
using miranaSolution.DTOs.Common;
using miranaSolution.DTOs.Core.Bookmarks;
using miranaSolution.DTOs.Core.Comments;
using miranaSolution.Services.Core.Comments;
using miranaSolution.Services.Exceptions;
using miranaSolution.Services.Validations;
using Moq;
using Xunit;

namespace miranaSolution.Services.Tests.Core;

public class CommentServiceTests
{
    private readonly CommentService _commentService;
    private readonly MiranaDbContext _context;
    private readonly Mock<IValidatorProvider> _validatorProviderMock = new();

    private readonly Mock<UserManager<AppUser>> _userManagerMock
        = new(Mock.Of<IUserStore<AppUser>>(),
            null!, null!, null!, null!,
            null!, null!, null!, null!);

    public CommentServiceTests()
    {
        var dbContextOptions = new DbContextOptionsBuilder<MiranaDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new MiranaDbContext(dbContextOptions);
        _commentService = new CommentService(_context,
            _validatorProviderMock.Object,
            _userManagerMock.Object);
    }

    [Fact]
    public async Task CreateBookCommentAsync_ShouldCreateNewComment_WhenBeingCalled()
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

        var actual = await _commentService.CreateBookCommentAsync(
            new CreateBookCommentRequest(
                book.Id,
                user.Id,
                "content",
                0));

        Assert.NotNull(actual.CommentVm);
        Assert.Equal("content", actual.CommentVm.Content);
        Assert.Equal(user.Id, actual.CommentVm.UserId);
        Assert.Equal(book.Id, actual.CommentVm.BookId);
    }

    [Fact]
    public async Task CreateBookCommentAsync_ShouldThrowUserNotFoundException_WhenBeingCalledWithInvalidUserId()
    {
        _userManagerMock
            .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(() => null!);

        await Assert.ThrowsAsync<UserNotFoundException>(
            async () => await _commentService.CreateBookCommentAsync(
                new CreateBookCommentRequest(
                    1, Guid.NewGuid(), "", 0)));
    }

    [Fact]
    public async Task CreateBookCommentAsync_ShouldThrowBookNotFoundException_WhenBeingCalledWithInvalidBookId()
    {
        _userManagerMock
            .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(new AppUser());

        await Assert.ThrowsAsync<BookNotFoundException>(
            async () => await _commentService.CreateBookCommentAsync(
                new CreateBookCommentRequest(
                    1, Guid.NewGuid(), "", 0)));
    }

    [Fact]
    public async Task DeleteBookCommentAsync_ShouldDeleteExistingComment_BeingCalled()
    {
        var user = new AppUser
        {
            Id = Guid.NewGuid(),
        };
        var book = new Book
        {
            Id = 1,
        };

        var comment = new Comment
        {
            Id = 1,
            UserId = user.Id,
            BookId = book.Id
        };
        
        _userManagerMock
            .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(user);
        
        await _context.Books.AddAsync(book);
        await _context.Users.AddAsync(user);
        await _context.Comments.AddAsync(comment);
        await _context.SaveChangesAsync();

        await _commentService.DeleteBookCommentAsync(
            new DeleteBookCommentRequest(comment.Id, book.Id, user.Id, false));
        
        Assert.Empty(await _context.Comments.ToListAsync());
    }

    [Fact] 
    public async Task DeleteBookCommentAsync_ShouldDeleteExistingComment_BeingCalledWithForceDeleteEqualToTrue()
    {
        var user = new AppUser
        {
            Id = Guid.NewGuid(),
        };
        var book = new Book
        {
            Id = 1,
        };

        var comment = new Comment
        {
            Id = 1,
            UserId = user.Id,
            BookId = book.Id
        };
        
        _userManagerMock
            .Setup(x => x.FindByIdAsync(user.Id.ToString()))
            .ReturnsAsync(user);
        
        await _context.Books.AddAsync(book);
        await _context.Users.AddAsync(user);
        await _context.Comments.AddAsync(comment);
        await _context.SaveChangesAsync();

        await _commentService.DeleteBookCommentAsync(
            new DeleteBookCommentRequest(comment.Id, book.Id, Guid.NewGuid(), true));
        
        Assert.Empty(await _context.Comments.ToListAsync());
    }
    
    [Fact] 
    public async Task DeleteBookCommentAsync_ShouldThrowBookNotFoundException_WhenBeingCalledWithInvalidBookId()
    {
        await Assert.ThrowsAsync<BookNotFoundException>(
            async () => await _commentService.DeleteBookCommentAsync(
                new DeleteBookCommentRequest(
                    1, 1, Guid.NewGuid(), true)));
    }
    
    
    [Fact] 
    public async Task DeleteBookCommentAsync_ShouldThrowUserNotFoundException_WhenBeingCalledWithInvalidUserId()
    {
        var user = new AppUser
        {
            Id = Guid.NewGuid(),
        };
        var book = new Book
        {
            Id = 1,
        };

        var comment = new Comment
        {
            Id = 1,
            UserId = user.Id,
            BookId = book.Id
        };

        _userManagerMock
            .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(() => null!);
        await _context.Books.AddAsync(book);
        await _context.Comments.AddAsync(comment);
        await _context.SaveChangesAsync();
        
        await Assert.ThrowsAsync<UserNotFoundException>(
            async () => await _commentService.DeleteBookCommentAsync(
                new DeleteBookCommentRequest(
                    comment.Id, book.Id, Guid.NewGuid(), false)));
    }

    [Fact]
    public async Task DeleteBookCommentAsync_ShouldThrowCommentNotFoundException_WhenBeingCalledWithInvalidCommentId()
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
            .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(user);
        await _context.Books.AddAsync(book);
        await _context.SaveChangesAsync();
        
        await Assert.ThrowsAsync<CommentNotFoundException>(
            async () => await _commentService.DeleteBookCommentAsync(
                new DeleteBookCommentRequest(
                    1, book.Id, Guid.NewGuid(), false)));
    }

    [Fact]
    public async Task GetBookCommentByIdAsync_ShouldReturnABook_WhenBeingCalled()
    {
        var user = new AppUser
        {
            Id = Guid.NewGuid(),
        };
        var book = new Book
        {
            Id = 1,
        };

        var comment = new Comment
        {
            Id = 1,
            UserId = user.Id,
            BookId = book.Id
        };

        _userManagerMock
            .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(user);
        await _context.Books.AddAsync(book);
        await _context.Comments.AddAsync(comment);
        await _context.SaveChangesAsync();

        var actual = await _commentService.GetBookCommentByIdAsync(
            new GetBookCommentByIdRequest(comment.Id, book.Id));

        Assert.NotNull(actual);
        Assert.Equal(book.Id, actual.CommentVm.BookId);
        Assert.Equal(user.Id, actual.CommentVm.UserId);
    }

    [Fact]
    public async Task GetBookCommentByIdAsync_ShouldThrowBookNotFoundException_WhenBeingCalledWithInvalidBookId()
    {
        await Assert.ThrowsAsync<BookNotFoundException>(
            async () => await _commentService.GetBookCommentByIdAsync(
                new GetBookCommentByIdRequest(1, 1)));
    }

    [Fact]
    public async Task GetBookCommentByIdAsync_ShouldThrowCommentNotFoundException_WhenBeingCalledWithInvalidCommentId()
    {
        var book = new Book
        {
            Id = 1,
        };

        await _context.Books.AddAsync(book);
        await _context.SaveChangesAsync();

        await Assert.ThrowsAsync<CommentNotFoundException>(
            async () => await _commentService.GetBookCommentByIdAsync(
                new GetBookCommentByIdRequest(
                    1, 1)));
    }

    [Fact]
    public async Task CountCommentByUserIdAsync_ShouldReturnNumberOfComments_WhenBeingCalled()
    {
        var user = new AppUser
        {
            Id = Guid.NewGuid(),
        };
        var book = new Book
        {
            Id = 1,
        };

        var comment = new Comment
        {
            Id = 1,
            UserId = user.Id,
            BookId = book.Id
        };

        _userManagerMock
            .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(user);
        await _context.Books.AddAsync(book);
        await _context.Comments.AddAsync(comment);
        await _context.SaveChangesAsync();

        var actual = await _commentService.CountCommentByUserIdAsync(
            new CountCommentByUserIdRequest(user.Id));

        Assert.Equal(1, actual.TotalComments);
    }

    [Fact]
    public async Task CountCommentByUserIdAsync_ShouldThrowUserNotFoundException_WhenBeingCalledWithInvalidUserId()
    {
        var user = new AppUser
        {
            Id = Guid.NewGuid(),
        };
        var book = new Book
        {
            Id = 1,
        };

        var comment = new Comment
        {
            Id = 1,
            UserId = user.Id,
            BookId = book.Id
        };

        _userManagerMock
            .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(() => null!);
        await _context.Books.AddAsync(book);
        await _context.Comments.AddAsync(comment);
        await _context.SaveChangesAsync();

        await Assert.ThrowsAsync<UserNotFoundException>(
            async () => await _commentService.CountCommentByUserIdAsync(
                new CountCommentByUserIdRequest(
                    user.Id)));
    }

    [Fact]
    public async Task GetAllBookCommentsAsync_ShouldReturnAllComments_WhenBeingCalled()
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

        var comment1 = new Comment
        {
            Id = 1,
            UserId = user1.Id,
            BookId = book.Id,
            CreatedAt = DateTime.Now.AddMinutes(1)
        };
        var comment2 = new Comment
        {
            Id = 2,
            UserId = user2.Id,
            BookId = book.Id,
            CreatedAt = DateTime.Now
        };
        var comment3 = new Comment
        {
            Id = 3,
            UserId = user2.Id,
            BookId = book.Id,
            ParentId = 1
        };

        await _context.Users.AddRangeAsync(user1, user2);
        await _context.Books.AddAsync(book);
        await _context.Comments.AddRangeAsync(comment1, comment2, comment3);
        await _context.SaveChangesAsync();

        var actual1 = await _commentService.GetAllBookCommentsAsync(
            new GetAllBookCommentsRequest(book.Id, null, null, 
                new PagerRequest(1, 10)));

        Assert.NotEmpty(actual1.CommentVms);
        Assert.Equal(2, actual1.CommentVms.Count);
        Assert.Collection(actual1.CommentVms,
            x => Assert.Equal(comment1.Id, x.Id),
            x => Assert.Equal(comment2.Id, x.Id));
        
        var actual2 = await _commentService.GetAllBookCommentsAsync(
            new GetAllBookCommentsRequest(book.Id, 1, null, 
                new PagerRequest(1, 10)));
        
        Assert.NotEmpty(actual2.CommentVms);
        Assert.Single(actual2.CommentVms);
        Assert.Collection(actual2.CommentVms,
            x => Assert.Equal(comment3.Id, x.Id));
        
        var actual3 = await _commentService.GetAllBookCommentsAsync(
            new GetAllBookCommentsRequest(book.Id, null, true, 
                new PagerRequest(1, 10)));
        
        Assert.NotEmpty(actual3.CommentVms);
        Assert.Equal(2, actual3.CommentVms.Count);
        Assert.Collection(actual3.CommentVms,
            x => Assert.Equal(comment2.Id, x.Id),
            x => Assert.Equal(comment1.Id, x.Id));
        
        Assert.Equal(1, actual1.PagerResponse.PageIndex);
        Assert.Equal(10, actual1.PagerResponse.PageSize);
    }
    
    [Fact]
    public async Task GetAllBookCommentsAsync_ShouldThrowBookNotFoundException_WhenBeingCalledWithInvalidBookId()
    {
        await Assert.ThrowsAsync<BookNotFoundException>(
            async () => await _commentService.GetAllBookCommentsAsync(
                new GetAllBookCommentsRequest(1, null, null, new PagerRequest(1,1))));
    }
}
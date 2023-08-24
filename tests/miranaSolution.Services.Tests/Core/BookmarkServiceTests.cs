using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using miranaSolution.Data.Entities;
using miranaSolution.Data.Main;
using miranaSolution.DTOs.Core.Bookmarks;
using miranaSolution.DTOs.Core.Chapters.Exceptions;
using miranaSolution.Services.Core.Bookmarks;
using miranaSolution.Services.Exceptions;
using Moq;
using Xunit;

namespace miranaSolution.Services.Tests.Core;

public class BookmarkServiceTests
{
    private readonly BookmarkService _bookmarkService;
    private readonly MiranaDbContext _context;

    private readonly Mock<UserManager<AppUser>> _userManagerMock
        = new(Mock.Of<IUserStore<AppUser>>(),
            null!, null!, null!, null!, 
            null!, null!, null!, null!);

    public BookmarkServiceTests()
    {
        var dbContextOptions = new DbContextOptionsBuilder<MiranaDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new MiranaDbContext(dbContextOptions);
        _bookmarkService = new BookmarkService(_context, _userManagerMock.Object);
    }
        
    [Fact]
    public async Task CreateBookmarkAsync_ShouldCreateNewBookmark_WhenBeingCalled()
    {
        var userId = Guid.NewGuid();
        var user = new AppUser
        {
            Id = userId,
        };
        var book = new Book
        {
            Id = 1,
        };
        var chapter = new Chapter
        {
            Id = 1,
            Index = 1
        };
        
        book.Chapters.Add(chapter);
        
        _userManagerMock
            .Setup(x => x.FindByIdAsync(userId.ToString()))
            .ReturnsAsync(user);
        await _context.Books.AddAsync(book);
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        var actual = await _bookmarkService.CreateBookmarkAsync(
            new CreateBookmarkRequest(userId, 1, 1));

        Assert.NotNull(actual.BookmarkVm);
        Assert.Equal(userId, actual.BookmarkVm.UserId);
        Assert.Equal(book.Id, actual.BookmarkVm.BookId);
        Assert.Equal(chapter.Index, actual.BookmarkVm.ChapterIndex);
    }

    [Fact]
    public async Task CreateBookmarkAsync_ShouldThrowUserNotFoundException_WhenBeingCalledWithInvalidUserId()
    {
        _userManagerMock
            .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(() => null!);

        await Assert.ThrowsAsync<UserNotFoundException>(
            async () => await _bookmarkService.CreateBookmarkAsync(
                new CreateBookmarkRequest(Guid.NewGuid(), 1, 1)));
    }
    
    [Fact]
    public async Task CreateBookmarkAsync_ShouldThrowBookNotFoundException_WhenBeingCalledWithInvalidBookId()
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
            async () => await _bookmarkService.CreateBookmarkAsync(
                new CreateBookmarkRequest(Guid.NewGuid(), 1, 1)));
    }

    [Fact]
    public async Task CreateBookmarkAsync_ShouldUpdateExistingBookmarkWithNewChapterIndex_WhenBookmarkExists()
    {
        var userId = Guid.NewGuid();
        var user = new AppUser
        {
            Id = userId,
        };
        var book = new Book
        {
            Id = 1,
        };
        var chapter1 = new Chapter
        {
            Id = 1,
            Index = 1
        };
        var chapter2 = new Chapter
        {
            Id = 2,
            Index = 2
        };
        var bookmark = new Bookmark
        {
            ChapterIndex = 1,
            UserId = userId,
            BookId = 1
        };
        await _context.Bookmarks.AddAsync(bookmark);

        book.Chapters.Add(chapter1);
        book.Chapters.Add(chapter2);
        
        _userManagerMock
            .Setup(x => x.FindByIdAsync(userId.ToString()))
            .ReturnsAsync(user);
        await _context.Books.AddAsync(book);
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        _context.Entry(bookmark).State = EntityState.Detached;

        var actual = await _bookmarkService.CreateBookmarkAsync(
            new CreateBookmarkRequest(userId, 1, 2));

        Assert.NotNull(actual.BookmarkVm);
        Assert.Equal(userId, actual.BookmarkVm.UserId);
        Assert.Equal(book.Id, actual.BookmarkVm.BookId);
        Assert.Equal(chapter2.Index, actual.BookmarkVm.ChapterIndex);
        Assert.Single(await _context.Bookmarks.ToListAsync());
    }
    
    [Fact]
    public async Task CreateBookmarkAsync_ShouldThrowChapterNotFoundException_WhenBeingCalledWithInvalidChapterIndex()
    {
        var userId = Guid.NewGuid();
        var user = new AppUser
        {
            Id = userId,
        };
        _userManagerMock
            .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(user);
        var book = new Book
        {
            Id = 1,
        };
        await _context.Books.AddAsync(book);
        await _context.SaveChangesAsync();

        await Assert.ThrowsAsync<ChapterNotFoundException>(
            async () => await _bookmarkService.CreateBookmarkAsync(
                new CreateBookmarkRequest(userId, 1, 1)));
    }

    [Fact]
    public async Task DeleteBookmarkAsync_ShouldDeleteExistingBookmark_WhenBeingCalled()
    {
        var userId = Guid.NewGuid();
        var user = new AppUser
        {
            Id = userId,
        };
        await _context.Users.AddAsync(user);
        var book = new Book
        {
            Id = 1,
        };
        await _context.Books.AddAsync(book);
        var chapter = new Chapter
        {
            Id = 1,
            Index = 1
        };
        book.Chapters.Add(chapter);
        
        var bookmark = new Bookmark
        {
            ChapterIndex = 1,
            UserId = userId,
            BookId = 1
        };
        await _context.Bookmarks.AddAsync(bookmark);

        _userManagerMock
            .Setup(x => x.FindByIdAsync(userId.ToString()))
            .ReturnsAsync(user);
        
        await _context.SaveChangesAsync();
        _context.Entry(bookmark).State = EntityState.Detached;

        await _bookmarkService.DeleteBookmarkAsync(
            new DeleteBookmarkRequest(userId, 1));
        
        Assert.Equal(0, await _context.Bookmarks.CountAsync());
    }

    [Fact]
    public async Task DeleteBookmarkAsync_ShouldThrowUserNotFoundException_WhenBeingCalledWithInvalidUserId()
    {
        _userManagerMock
            .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(() => null!);

        await Assert.ThrowsAsync<UserNotFoundException>(
            async () => await _bookmarkService.DeleteBookmarkAsync(
                new DeleteBookmarkRequest(Guid.NewGuid(), 1)));
    }
    
    [Fact]
    public async Task DeleteBookmarkAsync_ShouldThrowBookNotFoundException_WhenBeingCalledWithInvalidBookId()
    {
        _userManagerMock
            .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(new AppUser());

        await Assert.ThrowsAsync<BookNotFoundException>(
            async () => await _bookmarkService.DeleteBookmarkAsync(
                new DeleteBookmarkRequest(Guid.NewGuid(), 1)));
    }

    [Fact]
    public async Task
        DeleteBookmarkAsync_ShouldThrowBookmarkNotFoundException_WhenNoBookmarkExists()
    {
        _userManagerMock
            .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(new AppUser());
        var book = new Book
        {
            Id = 1,
        };
        await _context.Books.AddAsync(book);
        await _context.SaveChangesAsync();
        
        await Assert.ThrowsAsync<BookmarkNotFoundException>(
            async () => await _bookmarkService.DeleteBookmarkAsync(
                new DeleteBookmarkRequest(Guid.NewGuid(), 1)));
    }

    [Fact]
    public async Task GetAllBookmarksByUserIdAsync_ShouldReturnAllBookmarkOfTheUser_WhenBeingCalled()
    {
        var user = new AppUser
        {
            Id = Guid.NewGuid(),
        };
        var book1 = new Book
        {
            Id = 1
        };
        var book2 = new Book
        {
            Id = 2
        };
        var bookmark1 = new Bookmark()
        {
            BookId = book1.Id,
            UserId = user.Id,
            ChapterIndex = 1
        };
        var bookmark2 = new Bookmark()
        {
            BookId = book2.Id,
            UserId = user.Id,
            ChapterIndex = 1
        };

        await _context.Users.AddAsync(user);
        await _context.Books.AddRangeAsync(new List<Book> { book1, book2 });
        await _context.Bookmarks.AddRangeAsync(new List<Bookmark> {bookmark1, bookmark2});
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();
        
        _userManagerMock
            .Setup(x => x.FindByIdAsync(user.Id.ToString()))
            .ReturnsAsync(user);

        var actual1 = await _bookmarkService.GetAllBookmarksByUserIdAsync(
            new GetAllBookmarksByUserIdRequest(user.Id, null));

        Assert.NotNull(actual1.BookmarkVms);
        Assert.Equal(2, actual1.BookmarkVms.Count);
        
        var actual2 = await _bookmarkService.GetAllBookmarksByUserIdAsync(
            new GetAllBookmarksByUserIdRequest(user.Id, book2.Id));
        
        Assert.NotNull(actual2.BookmarkVms);
        Assert.Single(actual2.BookmarkVms);
    }

    [Fact]
    public async Task GetAllBookmarksByUserIdAsync_ShouldThrowUserNotFoundException_WhenBeingCalledWithInvalidUserId()
    {
        _userManagerMock
            .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(() => null!);

        await Assert.ThrowsAsync<UserNotFoundException>(
            async () => await _bookmarkService.GetAllBookmarksByUserIdAsync(
                new(Guid.NewGuid(), null)));
    }
    
    [Fact]
    public async Task GetAllBookmarksByUserIdAsync_ShouldThrowBookNotFoundException_WhenBeingCalledWithInvalidBookId()
    {
        _userManagerMock
            .Setup(x => x.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(new AppUser());

        await Assert.ThrowsAsync<BookNotFoundException>(
            async () => await _bookmarkService.GetAllBookmarksByUserIdAsync(
                new(Guid.NewGuid(), 1)));
    }

    [Fact]
    public async Task GetAllBookmarksByBookIdAsync_ShouldReturnAllBookmarkOfBook_WhenBeingCalled()
    {
        var user = new AppUser
        {
            Id = Guid.NewGuid(),
            UserName = "UserName1",
        };
        var book1 = new Book
        {
            Id = 1
        };
        var bookmark1 = new Bookmark()
        {
            BookId = book1.Id,
            UserId = user.Id,
            ChapterIndex = 1
        };

        await _context.Users.AddAsync(user);
        await _context.Books.AddAsync(book1);
        await _context.Bookmarks.AddAsync(bookmark1);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        var actual = await _bookmarkService.GetAllBookmarksByBookIdAsync(
            new GetAllBookmarksByBookIdRequest(book1.Id));

        Assert.NotNull(actual.BookmarkVms);
        Assert.Single(actual.BookmarkVms);
    }

    [Fact]
    public async Task GetAllBookmarksByBookIdAsync_ShouldThrowBookNotFoundException_WhenBeingCalledWithInvalidBookId()
    {
        await Assert.ThrowsAsync<BookNotFoundException>(
            async () => await _bookmarkService.GetAllBookmarksByBookIdAsync(
                new GetAllBookmarksByBookIdRequest(1)));
    }
    
}
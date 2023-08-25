using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using miranaSolution.Data.Entities;
using miranaSolution.Data.Main;
using miranaSolution.Services.Core.Bookmarks;
using miranaSolution.Services.Core.CurrentlyReadings;
using Moq;
using Xunit;

namespace miranaSolution.Services.Tests.Core;

public class CurrentlyReadingServiceTests
{
    private readonly CurrentlyReadingService _currentlyReadingService;
    private readonly MiranaDbContext _context;
    private readonly Mock<UserManager<AppUser>> _userManagerMock
        = new(Mock.Of<IUserStore<AppUser>>(),
            null!, null!, null!, null!, 
            null!, null!, null!, null!);
    
    public CurrentlyReadingServiceTests()
    {
        var dbContextOptions = new DbContextOptionsBuilder<MiranaDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new MiranaDbContext(dbContextOptions);
        _currentlyReadingService = new CurrentlyReadingService(_context, _userManagerMock.Object);
    }

    [Fact]
    public async Task AddBookAsync_ShouldAddABook_WhenBeingCalled()
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
    }
}
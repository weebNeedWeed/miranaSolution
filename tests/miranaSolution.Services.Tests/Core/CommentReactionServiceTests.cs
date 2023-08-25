using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using miranaSolution.Data.Entities;
using miranaSolution.Data.Main;
using miranaSolution.DTOs.Core.CommentReactions;
using miranaSolution.Services.Core.Authors;
using miranaSolution.Services.Core.CommentReactions;
using miranaSolution.Services.Exceptions;
using Moq;
using Xunit;

namespace miranaSolution.Services.Tests.Core;

public class CommentReactionServiceTests
{
    private readonly CommentReactionService _commentReactionService;
    private readonly MiranaDbContext _context;
    private readonly Mock<UserManager<AppUser>> _userManagerMock = new(
        Mock.Of<IUserStore<AppUser>>(),
        null!, null!, null!, null!,
        null!, null!, null!, null!);
    
    public CommentReactionServiceTests()
    {
        var dbContextOptions = new DbContextOptionsBuilder<MiranaDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new MiranaDbContext(dbContextOptions);
        _commentReactionService = new CommentReactionService(_context, _userManagerMock.Object);
    }

    [Fact]
    public async Task CreateCommentReactionAsync_ShouldCreateReaction_WhenBeingCalled()
    {
        var comment = new Comment
        {
            Id = 1,
        };
        var user = new AppUser
        {
            Id = Guid.NewGuid(),
        };

        await _context.Comments.AddAsync(comment);
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        var actual = await _commentReactionService.CreateCommentReactionAsync(
            new CreateCommentReactionRequest(user.Id, comment.Id));

        Assert.NotNull(actual.CommentReactionVm);
    }

    [Fact]
    public async Task CreateCommentReactionAsync_ShouldThrowUserAlreadyReactsToCommentException_WhenUserAlreadyReacted()
    {
        var comment = new Comment
        {
            Id = 1,
        };
        var user = new AppUser
        {
            Id = Guid.NewGuid(),
        };
        var reaction = new CommentReaction
        {
            UserId = user.Id,
            CommentId = comment.Id
        };

        await _context.Comments.AddAsync(comment);
        await _context.Users.AddAsync(user);
        await _context.CommentReactions.AddAsync(reaction);
        await _context.SaveChangesAsync();
        
        await Assert.ThrowsAsync<UserAlreadyReactsToCommentException>(
            async () => await _commentReactionService.CreateCommentReactionAsync(
                new (user.Id, comment.Id)));
    }

    [Fact]
    public async Task DeleteCommentReactionAsync_ShouldDeleteReaction_WhenBeingCalled()
    {
        var comment = new Comment
        {
            Id = 1,
        };
        var user = new AppUser
        {
            Id = Guid.NewGuid(),
        };
        var reaction = new CommentReaction
        {
            UserId = user.Id,
            CommentId = comment.Id
        };
        
        await _context.Comments.AddAsync(comment);
        await _context.Users.AddAsync(user);
        await _context.CommentReactions.AddAsync(reaction);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        await _commentReactionService.DeleteCommentReactionAsync(
            new DeleteCommentReactionRequest(user.Id, comment.Id));
        
        Assert.Empty(await _context.CommentReactions.ToListAsync());
    }

    [Fact]
    public async Task DeleteCommentReactionAsync_ShouldThrowUserNotReactedToCommentException_WhenNoReactionExists()
    {
        await Assert.ThrowsAsync<UserNotReactedToCommentException>(
            async () => await _commentReactionService.DeleteCommentReactionAsync(
                new (Guid.NewGuid(), 1)));
    }

    [Fact]
    public async Task CountCommentReactionByCommentIdAsync_ShouldReturnNumberOfReaction_WhenBeingCalled()
    {
        var comment = new Comment
        {
            Id = 1,
        };
        var user = new AppUser
        {
            Id = Guid.NewGuid(),
        };
        var reaction = new CommentReaction
        {
            UserId = user.Id,
            CommentId = comment.Id
        };
        
        await _context.Comments.AddAsync(comment);
        await _context.Users.AddAsync(user);
        await _context.CommentReactions.AddAsync(reaction);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        var actual = await _commentReactionService
            .CountCommentReactionByCommentIdAsync(new CountCommentReactionByCommentIdRequest(comment.Id));
        
        Assert.Equal(1, actual.TotalReactions);
    }

    [Fact]
    public async Task
        CountCommentReactionByCommentIdAsync_ShouldThrowCommentNotFoundException_WhenBeingCalledWithInvalidCommentId()
    {
        await Assert.ThrowsAsync<CommentNotFoundException>(
            async () => await _commentReactionService.CountCommentReactionByCommentIdAsync(
                new (1)));
    }

    [Fact]
    public async Task CountCommentReactionByUserIdAsync_ShouldReturnNumberOfReactions_WhenBeingCalled()
    {
        var comment = new Comment
        {
            Id = 1,
        };
        var user = new AppUser
        {
            Id = Guid.NewGuid(),
        };
        var reaction = new CommentReaction
        {
            UserId = user.Id,
            CommentId = comment.Id
        };
        
        _userManagerMock
            .Setup(x => x.FindByIdAsync(user.Id.ToString()))
            .ReturnsAsync(user);
        
        await _context.Comments.AddAsync(comment);
        await _context.Users.AddAsync(user);
        await _context.CommentReactions.AddAsync(reaction);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        var actual = await _commentReactionService
            .CountCommentReactionByUserIdAsync(new (user.Id));
        
        Assert.Equal(1, actual.TotalReactions);
    }

    [Fact]
    public async Task
        CountCommentReactionByUserIdAsync_ShouldThrowUserNotFoundException_WhenBeingCalledWithInvalidUserId()
    {
        await Assert.ThrowsAsync<UserNotFoundException>(
            async () => await _commentReactionService.CountCommentReactionByUserIdAsync(
                new (Guid.NewGuid())));
    }

    [Fact]
    public async Task CheckUserIsReactedAsync_ShouldReturnTrue_WhenUserAlreadyReacted()
    {
        var comment = new Comment
        {
            Id = 1,
        };
        var user = new AppUser
        {
            Id = Guid.NewGuid(),
        };
        var reaction = new CommentReaction
        {
            UserId = user.Id,
            CommentId = comment.Id
        };
        
        _userManagerMock
            .Setup(x => x.FindByIdAsync(user.Id.ToString()))
            .ReturnsAsync(user);
        
        await _context.Comments.AddAsync(comment);
        await _context.Users.AddAsync(user);
        await _context.CommentReactions.AddAsync(reaction);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        var actual = await _commentReactionService
            .CheckUserIsReactedAsync(new (user.Id, comment.Id));
        
        Assert.True(actual.IsReacted);
    }
    
    [Fact]
    public async Task CheckUserIsReactedAsync_ShouldReturnFalse_WhenUserNotReacted()
    {
        var comment = new Comment
        {
            Id = 1,
        };
        var user = new AppUser
        {
            Id = Guid.NewGuid(),
        };

        _userManagerMock
            .Setup(x => x.FindByIdAsync(user.Id.ToString()))
            .ReturnsAsync(user);
        
        await _context.Comments.AddAsync(comment);
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        var actual = await _commentReactionService
            .CheckUserIsReactedAsync(new (user.Id, comment.Id));
        
        Assert.False(actual.IsReacted);
    }
}
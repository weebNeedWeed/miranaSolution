using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using miranaSolution.Data.Entities;
using miranaSolution.Data.Main;
using miranaSolution.DTOs.Core.Authors;
using miranaSolution.Services.Core.Authors;
using miranaSolution.Services.Exceptions;
using miranaSolution.Services.Validations;
using Moq;
using Xunit;

namespace miranaSolution.Services.Tests.Core;

public class AuthorServiceTests
{
    private readonly AuthorService _authorService;
    private readonly MiranaDbContext _context;
    private readonly Mock<IValidatorProvider> _validatorProviderMock = new Mock<IValidatorProvider>();

    public AuthorServiceTests()
    {
        var dbContextOptions = new DbContextOptionsBuilder<MiranaDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new MiranaDbContext(dbContextOptions);
        _authorService = new AuthorService(_context, _validatorProviderMock.Object);
    }

    [Fact]
    public async Task GetAllAuthorsAsync_ShouldReturnListOfAuthorVm_WhenBeingCalled()
    {
        // Arrange
        var author1 = new Author
        {
            Id = 1,
            Name = "Author1"
        };
        var author2 = new Author
        {
            Id = 2,
            Name = "Author2"
        };
        var authorList = new List<Author>() { author1, author2 };
        await _context.Authors.AddRangeAsync(authorList);
        await _context.SaveChangesAsync();
    
        // Act
        var actual = await _authorService.GetAllAuthorsAsync();
    
        // Assert
        Assert.NotNull(actual.AuthorVms);
        Assert.Equal(2, actual.AuthorVms.Count);
    }

    [Fact]
    public async Task CreateAuthorAsync_ShouldCreateNewAuthor_WhenBeingCalled()
    {
        // Arrange
        var request = new CreateAuthorRequest(
            "author1");
        
        // Act
        var actual = await _authorService.CreateAuthorAsync(request);
        
        // Assert
        Assert.NotNull(actual.AuthorVm);
        Assert.Equal(request.Name, actual.AuthorVm.Name);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData("ath")]
    public async Task CreateAuthorAsync_ShouldThrowAValidationException_WhenBeingCalledWithInvalidParameters(string name)
    {
        var request = new CreateAuthorRequest(
            name);
        _validatorProviderMock.Setup(x => x.Validate(request))
            .Throws(new ValidationException(new List<ValidationFailure>()));

        await Assert.ThrowsAsync<ValidationException>(
            async () => await _authorService.CreateAuthorAsync(request));
    }

    [Fact]
    public async Task DeleteAuthorAsync_ShouldDeleteAnAuthor_WhenBeingCalled()
    {
        var author1 = new Author
        {
            Id = 1,
            Name = "Author1"
        };
        await _context.Authors.AddAsync(author1);
        await _context.SaveChangesAsync();

        await _authorService.DeleteAuthorAsync(
            new DeleteAuthorRequest(1));
        
        Assert.Equal(0, await _context.Authors.CountAsync());
    }
    
    [Fact]
    public async Task DeleteAuthorAsync_ShouldThrowAnException_WhenBeingCalledWithInvalidId()
    {
        var request = new DeleteAuthorRequest(1);
        
        await Assert.ThrowsAsync<AuthorNotFoundException>(
    async () => await _authorService.DeleteAuthorAsync(request));
    }

    [Fact]
    public async Task UpdateAuthorAsync_ShouldUpdateExistingAuthor_WhenBeingCalled()
    {
        var author1 = new Author
        {
            Id = 1,
            Name = "Author1"
        };
        await _context.Authors.AddAsync(author1);
        await _context.SaveChangesAsync();

        var newName = "author2";

        var actual = await _authorService.UpdateAuthorAsync(
            new UpdateAuthorRequest(1, newName));

        Assert.NotNull(actual.AuthorVm);
        Assert.Equal(newName, actual.AuthorVm.Name);
    }

    [Fact]
    public async Task UpdateAuthorAsync_ShouldThrowAnException_WhenBeingCalledWithInvalidId()
    {
        var request = new UpdateAuthorRequest(1, "author1");
        
        await Assert.ThrowsAsync<AuthorNotFoundException>(
            async () => await _authorService.UpdateAuthorAsync(request));
    }

    [Fact]
    public async Task GetAllBookByAuthorIdAsync_ShouldReturnAllBooks_WhenBeingCalled()
    {
        var author = new Author
        {
            Id = 1,
            Name = "Author1"
        };
        var book1 = new Book
        {
            Name = "book1",
            IsRecommended = true,
            IsDone = true,
        };
        var book2 = new Book
        {
            Name = "book2",
            IsRecommended = true,
            IsDone = true,
        };
        
        author.Books.AddRange(new List<Book>{book1, book2});
        await _context.Authors.AddAsync(author);
        await _context.SaveChangesAsync();

        var actual = await _authorService.GetAllBookByAuthorIdAsync(
            new GetAllBooksByAuthorIdRequest(1, 10));

        Assert.NotNull(actual.BookVms);
        Assert.Equal(2, actual.BookVms.Count);
    }
    
    [Fact]
    public async Task GetAllBookByAuthorIdAsync_ShouldThrowAnException_WhenBeingCalledWithInvalidId()
    {
        await Assert.ThrowsAsync<AuthorNotFoundException>(
            async () => await _authorService.GetAllBookByAuthorIdAsync(
                new GetAllBooksByAuthorIdRequest(1, 10)));
    }
}
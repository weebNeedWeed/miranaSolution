using Microsoft.EntityFrameworkCore;
using miranaSolution.Data.Main;
using miranaSolution.Services.Core.BookUpvotes;
using miranaSolution.Services.Core.Chapters;
using miranaSolution.Services.Validations;
using Moq;

namespace miranaSolution.Services.Tests.Core;

public class ChapterServiceTests
{
    private readonly ChapterService _chapterService;
    private readonly MiranaDbContext _context;
    private readonly Mock<IValidatorProvider> _validatorProviderMock = new();

    public ChapterServiceTests()
    {
        var dbContextOptions = new DbContextOptionsBuilder<MiranaDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new MiranaDbContext(dbContextOptions);
        _chapterService = new ChapterService(_context, _validatorProviderMock.Object);
    }
}
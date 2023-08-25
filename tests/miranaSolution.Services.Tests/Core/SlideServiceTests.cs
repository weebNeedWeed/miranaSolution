using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using miranaSolution.Data.Entities;
using miranaSolution.Data.Main;
using miranaSolution.DTOs.Core.Books;
using miranaSolution.DTOs.Core.Slides;
using miranaSolution.Services.Core.Books;
using miranaSolution.Services.Core.Slides;
using miranaSolution.Services.Exceptions;
using miranaSolution.Services.Systems.Images;
using miranaSolution.Services.Validations;
using Moq;
using Xunit;

namespace miranaSolution.Services.Tests.Core;

public class SlideServiceTests
{
    private readonly SlideService _slideService;
    private readonly MiranaDbContext _context;
    private readonly Mock<IValidatorProvider> _validatorProviderMock = new();
    private readonly Mock<IImageSaver> _imageSaverMock = new();

    public SlideServiceTests()
    {
        var dbContextOptions = new DbContextOptionsBuilder<MiranaDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new MiranaDbContext(dbContextOptions);
        _slideService = new SlideService(_context,
            _validatorProviderMock.Object,
            _imageSaverMock.Object);
    }

    [Fact]
    public async Task CreateSlideAsync_ShouldCreateNewSlide_WhenBeingCalled()
    {
        const string newName = "newName";
        const string des = "des";
        const int sortOrder = 1;

        _imageSaverMock.Setup(
                x => x.SaveImageAsync(
                    It.IsAny<Stream>(),
                    It.IsAny<string>()))
            .ReturnsAsync("some/path");

        var actual = await _slideService.CreateSlideAsync(
            new CreateSlideRequest(
                newName,
                des,
                "",
                sortOrder,
                Stream.Null,
                ""));

        Assert.NotNull(actual.SlideVm);
        Assert.Equal(newName, actual.SlideVm.Name);
        Assert.Equal(des, actual.SlideVm.ShortDescription);
        Assert.Equal(sortOrder, actual.SlideVm.SortOrder);
        Assert.Equal("some/path", actual.SlideVm.ThumbnailImage);
    }

    [Fact]
    public async Task CreateSlideAsync_ShouldThrowValidationException_WhenBeingCalledWithInvalidParameters()
    {
        _validatorProviderMock.Setup(x => x.Validate(It.IsAny<CreateSlideRequest>()))
            .Throws(new ValidationException(new List<ValidationFailure>()));

        await Assert.ThrowsAsync<ValidationException>(
            async () => await _slideService.CreateSlideAsync(
                new CreateSlideRequest(
                    "",
                    "",
                    "",
                    1,
                    Stream.Null,
                    "")));
    }

    [Fact]
    public async Task UpdateSlideAsync_ShouldUpdateExistingSlide_WhenBeingCalled()
    {
        var slide = new Slide
        {
            Id = 1
        };

        await _context.Slides.AddAsync(slide);
        await _context.SaveChangesAsync();

        const string newName = "newName";
        const string des = "des";
        const int sortOrder = 1;

        _imageSaverMock.Setup(
                x => x.SaveImageAsync(
                    It.IsAny<Stream>(),
                    It.IsAny<string>()))
            .ReturnsAsync("some/path");

        var actual = await _slideService.UpdateSlideAsync(
            new UpdateSlideRequest(
                slide.Id,
                newName,
                des,
                "",
                sortOrder,
                Stream.Null,
                ""));

        Assert.NotNull(actual.SlideVm);
        Assert.Equal(newName, actual.SlideVm.Name);
        Assert.Equal(des, actual.SlideVm.ShortDescription);
        Assert.Equal(sortOrder, actual.SlideVm.SortOrder);
        Assert.Equal("some/path", actual.SlideVm.ThumbnailImage);
    }

    [Fact]
    public async Task UpdateSlideAsync_ShouldThrowSlideNotFoundException_WhenBeingCalledWithInvalidSlideId()
    {
        await Assert.ThrowsAsync<SlideNotFoundException>(
            async () => await _slideService.UpdateSlideAsync(
                new UpdateSlideRequest(
                    1,
                    "",
                    "",
                    "",
                    1,
                    Stream.Null,
                    "")));
    }

    [Fact]
    public async Task UpdateSlideAsync_ShouldThrowValidationException_WhenBeingCalledWithInvalidParameters()
    {
        _validatorProviderMock.Setup(x => x.Validate(It.IsAny<UpdateSlideRequest>()))
            .Throws(new ValidationException(new List<ValidationFailure>()));

        await Assert.ThrowsAsync<ValidationException>(
            async () => await _slideService.UpdateSlideAsync(
                new UpdateSlideRequest(
                    1,
                    "",
                    "",
                    "",
                    1,
                    Stream.Null,
                    "")));
    }

    [Fact]
    public async Task DeleteSlideAsync_ShouldDeleteExistingSlide_WhenBeingCalled()
    {
        var slide = new Slide
        {
            Id = 1
        };

        await _context.Slides.AddAsync(slide);
        await _context.SaveChangesAsync();

        await _slideService.DeleteSlideAsync(
            new DeleteSlideRequest(slide.Id));

        Assert.Empty(await _context.Slides.ToListAsync());
    }

    [Fact]
    public async Task DeleteSlideAsync_ShouldThrowSlideNotFoundException_WhenBeingCalledWithInvalidSlideId()
    {
        await Assert.ThrowsAsync<SlideNotFoundException>(
            async () => await _slideService.DeleteSlideAsync(
                new DeleteSlideRequest(1)));
    }

    [Fact]
    public async Task GetAllSlidesAsync_ShouldReturnAllSlides_WhenBeingCalled()
    {
        var slide1 = new Slide
        {
            Id = 1
        };
        var slide2 = new Slide
        {
            Id = 2
        };

        await _context.Slides.AddRangeAsync(new List<Slide> {slide1, slide2});
        await _context.SaveChangesAsync();

        var actual = await _slideService.GetAllSlidesAsync();

        Assert.NotNull(actual.SlideVms);
        Assert.Equal(2, actual.SlideVms.Count);
    }

    [Fact]
    public async Task GetAllSlidesAsync_ShouldReturnASlide_WhenBeingCalledWithSlideId()
    {
        var slide = new Slide
        {
            Id = 1
        };

        await _context.Slides.AddAsync(slide);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        var actual = await _slideService.GetSlideByIdAsync(
            new GetSlideByIdRequest(slide.Id));
        
        Assert.NotNull(actual.SlideVm);
        Assert.Equal(slide.Id, actual.SlideVm.Id);
    }
    
    [Fact]
    public async Task GetAllSlidesAsync_ShouldReturnNull_WhenBeingCalledWithInvalidSlideId()
    {
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        var actual = await _slideService.GetSlideByIdAsync(
            new GetSlideByIdRequest(1));
        
        Assert.Null(actual.SlideVm);
    }
}
using Microsoft.EntityFrameworkCore;
using miranaSolution.Data.Entities;
using miranaSolution.Data.Main;
using miranaSolution.DTOs.Core.Slides;
using miranaSolution.Services.Exceptions;
using miranaSolution.Services.Systems.Images;
using miranaSolution.Services.Validations;

namespace miranaSolution.Services.Core.Slides;

public class SlideService : ISlideService
{
    private readonly MiranaDbContext _context;
    private readonly IValidatorProvider _validatorProvider;
    private readonly IImageSaver _imageSaver;

    public SlideService(MiranaDbContext context, IValidatorProvider validatorProvider, IImageSaver imageSaver)
    {
        _context = context;
        _validatorProvider = validatorProvider;
        _imageSaver = imageSaver;
    }

    public async Task<CreateSlideResponse> CreateSlideAsync(CreateSlideRequest request)
    {
        _validatorProvider.Validate(request);

        var slide = new Slide
        {
            Name = request.Name,
            Genres = request.Genres,
            ShortDescription = request.ShortDescription,
            SortOrder = request.SortOrder,
            ThumbnailImage = await _imageSaver.SaveImageAsync(
                request.ThumbnailImage, request.ThumbnailImageExtension)
        };

        await _context.Slides.AddAsync(slide);
        await _context.SaveChangesAsync();
        
        return new CreateSlideResponse(MapSlideToSlideVm(slide));
    }

    public async Task<UpdateSlideResponse> UpdateSlideAsync(UpdateSlideRequest request)
    {
        _validatorProvider.Validate(request);

        var slide = await _context.Slides.FindAsync(request.SlideId);
        if (slide is null)
        {
            throw new SlideNotFoundException("The slide with given Id does not exist.");
        }

        slide.Name = request.Name;
        slide.ShortDescription = request.ShortDescription;
        slide.SortOrder = request.SortOrder;
        slide.Genres = request.Genres;

        if (request.ThumbnailImage is not null)
        {
            await _imageSaver.DeleteImageIfExistAsync(slide.ThumbnailImage);
            slide.ThumbnailImage = await _imageSaver.SaveImageAsync(
                request.ThumbnailImage,
                request.ThumbnailImageExtension!);
        }

        await _context.SaveChangesAsync();

        return new UpdateSlideResponse(MapSlideToSlideVm(slide));
    }

    public async Task DeleteSlideAsync(DeleteSlideRequest request)
    {
        var slide = await _context.Slides.FindAsync(request.SlideId);
        if (slide is null)
        {
            throw new SlideNotFoundException("The slide with given Id does not exist.");
        }

        _context.Slides.Remove(slide);
        await _context.SaveChangesAsync();
    }

    public async Task<GetAllSlidesResponse> GetAllSlidesAsync()
    {
        var slides = await _context.Slides
            .OrderBy(x => x.SortOrder)
            .ToListAsync();

        var slideVms = slides.Select(MapSlideToSlideVm).ToList();

        return new GetAllSlidesResponse(slideVms);
    }

    public async Task<GetSlideByIdResponse> GetSlideByIdAsync(GetSlideByIdRequest request)
    {
        var slide = await _context.Slides.FindAsync(request.SlideId);
        if (slide is null) return new GetSlideByIdResponse(null);

        return new GetSlideByIdResponse(MapSlideToSlideVm(slide));
    }

    private SlideVm MapSlideToSlideVm(Slide slide)
    {
        return new SlideVm(
            slide.Id,
            slide.Name,
            slide.ShortDescription,
            slide.ThumbnailImage,
            slide.Genres,
            slide.SortOrder);
    }
}
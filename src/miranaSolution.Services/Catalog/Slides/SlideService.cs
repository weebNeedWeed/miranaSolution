﻿using Microsoft.EntityFrameworkCore;
using miranaSolution.Data.Entities;
using miranaSolution.Data.Main;
using miranaSolution.DTOs.Catalog.Slides;

namespace miranaSolution.Services.Catalog.Slides;

public class SlideService : ISlideService
{
    private readonly MiranaDbContext _context;

    public SlideService(MiranaDbContext context)
    {
        _context = context;
    }

    public async Task<CreateSlideResponse> CreateSlideAsync(CreateSlideRequest request)
    {
        var slide = new Slide
        {
            Name = request.Name,
            Genres = request.Genres,
            ShortDescription = request.ShortDescription,
            SortOrder = request.SortOrder,
            ThumbnailImage = request.ThumbnailImage
        };
        
        await _context.Slides.AddAsync(slide);
        await _context.SaveChangesAsync();

        var response = new CreateSlideResponse(
            new SlideVm(
                slide.Id,
                slide.Name,
                slide.ShortDescription,
                slide.ThumbnailImage,
                slide.Genres,
                slide.SortOrder));

        return response;
    }

    public async Task<GetAllSlidesResponse> GetAllSlidesAsync()
    {
        var slides = await _context.Slides
            .OrderBy(x => x.SortOrder)
            .Select(x => new SlideVm(
                x.Id,
                x.Name,
                x.ShortDescription,
                x.ThumbnailImage,
                x.Genres,
                x.SortOrder))
            .ToListAsync();
        
        var response = new GetAllSlidesResponse(slides);

        return response;
    }

    public async Task<GetSlideByIdResponse> GetSlideByIdAsync(GetSlideByIdRequest request)
    {
        var slide = await _context.Slides.FindAsync(request.SlideId);
        if (slide is null)
        {
            return new GetSlideByIdResponse(null);
        }
        
        var response = new GetSlideByIdResponse(
            new SlideVm(
                slide.Id,
                slide.Name,
                slide.ShortDescription,
                slide.ThumbnailImage,
                slide.Genres,
                slide.SortOrder));

        return response;
    }
}
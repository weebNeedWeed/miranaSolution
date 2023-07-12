using Microsoft.EntityFrameworkCore;
using miranaSolution.Data.Entities;
using miranaSolution.Data.Main;
using miranaSolution.DTOs.Catalog.Books;
using miranaSolution.DTOs.Catalog.Chapters;
using miranaSolution.DTOs.Catalog.Chapters.Exceptions;
using miranaSolution.DTOs.Common;
using miranaSolution.Services.Catalog.Books;
using miranaSolution.Services.Exceptions;
using miranaSolution.Utilities.Exceptions;

namespace miranaSolution.Services.Catalog.Chapters;

public class ChapterService : IChapterService
{
    private readonly MiranaDbContext _context;
    private readonly IBookService _bookService;

    public ChapterService(MiranaDbContext context, IBookService bookService)
    {
        _context = context;
        _bookService = bookService;
    }
    
    /// <exception cref="BookNotFoundException">
    /// Thrown when the book with given Id is not found
    /// </exception>
    public async Task<CreateChapterResponse> CreateChapterAsync(CreateChapterRequest request)
    {
        var getBookByIdResponse = await _bookService.GetBookByIdAsync(new GetBookByIdRequest(request.BookId));
        if (getBookByIdResponse.BookVm is null)
        {
            throw new BookNotFoundException(
                $"The book with given Id does not exist.");
        }
        
        // If there was no chapter added before, then prevIndex is 0 (the default value of int)
        var prevIndex = await _context.Chapters
            .Where(x => x.BookId == request.BookId)
            .OrderByDescending(x => x.Index)
            .Select(x => x.Index)
            .FirstOrDefaultAsync();

        var chapter = new Chapter
        {
            Name = request.Name,
            WordCount = request.WordCount,
            Content = request.Content,
            Index = prevIndex + 1,
            ReadCount = 0,
            BookId = request.BookId
        };
        
        await _context.Chapters.AddAsync(chapter);
        await _context.SaveChangesAsync();

        var getTotalChaptersResponse = await GetTotalChaptersAsync(new GetTotalChaptersRequest(request.BookId));

        var response = new CreateChapterResponse(
            new ChapterVm(
                chapter.Id,
                chapter.Index,
                chapter.Name,
                chapter.CreatedAt,
                chapter.UpdatedAt,
                chapter.ReadCount,
                chapter.WordCount,
                chapter.Content,
                chapter.Index < getTotalChaptersResponse.TotalChapters,
                chapter.Index > 1
            ));
        
        return response;
    }

    public async Task<GetAllChaptersResponse> GetAllChaptersAsync(GetAllChaptersRequest request)
    {
        var query = _context.Chapters.Where(x => x.BookId == request.BookId);
        query = query
            .Skip((request.PagerRequest.PageIndex - 1) * request.PagerRequest.PageSize)
            .Take(request.PagerRequest.PageSize)
            .OrderBy(x => x.Index);

        var getTotalChaptersResponse = await GetTotalChaptersAsync(new GetTotalChaptersRequest(request.BookId));
        var totalChapters = getTotalChaptersResponse.TotalChapters;
        
        var chapterVms = await query
            .Select(x => new ChapterVm(
                x.Id,
                x.Index,
                x.Name,
                x.CreatedAt,
                x.UpdatedAt,
                x.ReadCount,
                x.WordCount,
                x.Content,
                x.Index < totalChapters,
                x.Index > 1
            ))
            .ToListAsync();

        var response = new GetAllChaptersResponse(
            new PagerResponse(
                request.PagerRequest.PageIndex,
                request.PagerRequest.PageSize,
                totalChapters),
            chapterVms);

        return response;
    }
    
    /// <exception cref="ChapterNotFoundException">
    /// Thrown when the chapter with given Id is not found
    /// </exception>
    public async Task<GetChapterByIndexResponse> GetChapterByIndexAsync(GetChapterByIndexRequest request)
    {
        var chapter = await _context.Chapters
            .FirstOrDefaultAsync(x => x.BookId == request.BookId && x.Index == request.ChapterIndex);
        if (chapter is null)
        {
            throw new ChapterNotFoundException("The chapter with given Index does not exist.");
        }

        var getTotalChaptersResponse = await GetTotalChaptersAsync(new GetTotalChaptersRequest(request.BookId));

        var chapterVm = new ChapterVm(
            chapter.Id,
            chapter.Index,
            chapter.Name,
            chapter.CreatedAt,
            chapter.UpdatedAt,
            chapter.ReadCount,
            chapter.WordCount,
            chapter.Content,
            chapter.Index < getTotalChaptersResponse.TotalChapters,
            chapter.Index > 1);

        var response = new GetChapterByIndexResponse(chapterVm);

        return response;
    }

    public async Task<GetTotalChaptersResponse> GetTotalChaptersAsync(GetTotalChaptersRequest request)
    {
        var totalChapters = await _context.Chapters
            .Where(x => x.BookId == request.BookId)
            .CountAsync();

        var response = new GetTotalChaptersResponse(totalChapters);

        return response;
    }
}
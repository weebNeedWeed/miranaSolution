using AutoMapper;
using Microsoft.EntityFrameworkCore;
using miranaSolution.Data.Entities;
using miranaSolution.Data.Main;
using miranaSolution.DTOs.Catalog.Books.Chapters;
using miranaSolution.DTOs.Common;
using miranaSolution.Services.Catalog.Books;
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

    public async Task<ChapterDto> AddChapter(int bookId, ChapterCreateRequest request)
    {
        var book = await _bookService.GetById(bookId);
        if (book is null)
            throw new MiranaBusinessException(
                $"The book with Id: {bookId} is not found");

        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Chapter, ChapterDto>();
            cfg.CreateMap<ChapterCreateRequest, Chapter>();
        });
        var mapper = config.CreateMapper();

        // If there was no chapter added before, then prevIndex is 0 (default of int)
        var prevIndex = await _context.Chapters
            .Where(x => x.BookId == bookId)
            .OrderByDescending(x => x.Index)
            .Select(x => x.Index)
            .FirstOrDefaultAsync();

        var chapter = mapper.Map<Chapter>(request);
        chapter.Index = prevIndex + 1;
        chapter.ReadCount = 0;
        chapter.BookId = bookId;

        await _context.Chapters.AddAsync(chapter);
        await _context.SaveChangesAsync();

        var chapterDto = mapper.Map<ChapterDto>(chapter);

        return chapterDto;
    }

    public async Task<PagedResult<ChapterDto>> GetChaptersPaging(int bookId, ChapterGetPagingRequest request)
    {
        var query = _context.Chapters.Where(x => x.BookId == bookId);

        // if (!string.IsNullOrEmpty(request.Keyword))
        // {
        //     query = query.Where(x => x.Name.Contains(request.Keyword));
        // }

        query = query.Skip((request.PageIndex - 1) * request.PageSize)
            .Take(request.PageSize)
            .OrderBy(x => x.Index);

        var config = new MapperConfiguration(cfg => cfg.CreateMap<Chapter, ChapterDto>());
        var mapper = config.CreateMapper();

        var totalRows = await query.CountAsync();

        var data = await query.Select(x => mapper.Map<ChapterDto>(x)).ToListAsync();

        var result = new PagedResult<ChapterDto>
        {
            Items = data,
            PageIndex = request.PageIndex,
            PageSize = request.PageSize,
            TotalRecords = totalRows
        };

        return result;
    }

    public async Task<ChapterDto> GetChapterByIndex(int bookId, int index)
    {
        var chapter = await _context.Chapters.FirstOrDefaultAsync(x => x.BookId == bookId && x.Index == index);
        if (chapter is null) return null;

        var totalRows = await _context.Chapters.Where(x => x.BookId == bookId).CountAsync();

        var config = new MapperConfiguration(cfg => cfg.CreateMap<Chapter, ChapterDto>());
        var mapper = config.CreateMapper();

        var chapterDto = mapper.Map<ChapterDto>(chapter);
        chapterDto.TotalRecords = totalRows;

        return chapterDto;
    }
}
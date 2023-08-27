using Microsoft.EntityFrameworkCore;
using miranaSolution.Data.Entities;
using miranaSolution.Data.Main;
using miranaSolution.DTOs.Common;
using miranaSolution.DTOs.Core.Chapters;
using miranaSolution.Services.Exceptions;
using miranaSolution.Services.Validations;

namespace miranaSolution.Services.Core.Chapters;

public class ChapterService : IChapterService
{
    private readonly MiranaDbContext _context;
    private readonly IValidatorProvider _validatorProvider;

    public ChapterService(MiranaDbContext context, IValidatorProvider validatorProvider)
    {
        _context = context;
        _validatorProvider = validatorProvider;
    }

    /// <exception cref="BookNotFoundException">
    ///     Thrown when the book with given Id is not found
    /// </exception>
    public async Task<CreateBookChapterResponse> CreateBookChapterAsync(CreateBookChapterRequest request)
    {
        _validatorProvider.Validate(request);

        var book = await _context.Books.FindAsync(request.BookId);
        if (book is null)
            throw new BookNotFoundException("The book with given Id does not exist.");

        if (await _context.Chapters.AnyAsync(
                x => x.BookId == request.BookId && x.Index == request.Index))
        {
            throw new ChapterAlreadyExistsException("The book's chapter with given index already exists.");
        }

        var chapter = new Chapter
        {
            Name = request.Name,
            WordCount = request.WordCount,
            Content = request.Content,
            Index = request.Index,
            ReadCount = 0,
            BookId = request.BookId
        };

        await _context.Chapters.AddAsync(chapter);
        await _context.SaveChangesAsync();
        
        var response = new CreateBookChapterResponse(
            MapChapterToChapterVm(chapter));

        return response;
    }

    public async Task<GetAllBookChaptersResponse> GetAllBookChaptersAsync(GetAllBookChaptersRequest request)
    {
        var book = await _context.Books.FindAsync(request.BookId);
        if (book is null)
            throw new BookNotFoundException("The book with given Id does not exist.");
        
        var query = _context.Chapters
            .Where(x => x.BookId == request.BookId)
            .Skip((request.PagerRequest.PageIndex - 1) * request.PagerRequest.PageSize)
            .Take(request.PagerRequest.PageSize)
            .OrderBy(x => x.Index);

        var getTotalChaptersResponse = await GetTotalBookChaptersAsync(new GetTotalBookChaptersRequest(request.BookId));
        var totalChapters = getTotalChaptersResponse.TotalChapters;

        var chapters = await query
            .ToListAsync();
        var chapterVms = new List<ChapterVm>();

        foreach (var chapter in chapters)
        {
            var chapterVm = await MapTotallyChapterToChapterVmAsync(chapter);
            chapterVms.Add(chapterVm);
        }
        
        var response = new GetAllBookChaptersResponse(
            new PagerResponse(
                request.PagerRequest.PageIndex,
                request.PagerRequest.PageSize,
                totalChapters),
            chapterVms.ToList());

        return response;
    }

    /// <exception cref="BookNotFoundException">
    ///     Thrown when the book with given Id is not found
    /// </exception>
    public async Task<GetBookChapterByIndexResponse> GetBookChapterByIndexAsync(GetBookChapterByIndexRequest request)
    {
        var book = await _context.Books.FindAsync(request.BookId);
        if (book is null)
            throw new BookNotFoundException("The book with given Id does not exist.");

        var chapter = await _context.Chapters
            .FirstOrDefaultAsync(x => x.BookId == request.BookId && x.Index == request.ChapterIndex);
        if (chapter is null) return new GetBookChapterByIndexResponse(null);

        // Increase ViewCount of the book by 1
        book.ViewCount++;
        await _context.SaveChangesAsync();
        
        var response = new GetBookChapterByIndexResponse(
            await MapTotallyChapterToChapterVmAsync(chapter));

        return response;
    }

    public async Task<GetTotalBookChaptersResponse> GetTotalBookChaptersAsync(GetTotalBookChaptersRequest request)
    {
        var totalChapters = await _context.Chapters
            .Where(x => x.BookId == request.BookId)
            .CountAsync();

        var response = new GetTotalBookChaptersResponse(totalChapters);

        return response;
    }

    public async Task<GetLatestCreatedChaptersResponse> GetLatestCreatedChaptersAsync(
        GetLatestCreatedChaptersRequest request)
    {
        var query = _context.Chapters
            .OrderByDescending(x => x.CreatedAt)
            .Take(request.NumberOfChapters);

        var chapters = await query
            .ToListAsync();
        var chapterVms = chapters.Select(MapChapterToChapterVm).ToList();

        var response = new GetLatestCreatedChaptersResponse(chapterVms.ToList());
        return response;
    }

    public async Task DeleteBookChapterAsync(DeleteBookChapterRequest request)
    {
        var chapter = await _context.Chapters.FirstOrDefaultAsync(
            x => x.BookId == request.BookId && x.Index == request.Index);
        if (chapter is null)
        {
            throw new ChapterNotFoundException("The chapter with given index does not exist.");
        }

        if (await _context.ChapterRelationships.AnyAsync(
                x => x.FromId == chapter.Id
                     || x.ToId == chapter.Id))
        {
            throw new Exception("There is a need to remove the relationship to the next or previous chapter.");
        }

        _context.Chapters.Remove(chapter);
        await _context.SaveChangesAsync();
    }

    public async Task<UpdateBookChapterResponse> UpdateBookChapterAsync(UpdateBookChapterRequest request)
    {
        _validatorProvider.Validate(request);
        
        var chapter = await _context.Chapters.FirstOrDefaultAsync(
            x => x.BookId == request.BookId && x.Index == request.CurrentIndex);
        if (chapter is null)
        {
            throw new ChapterNotFoundException("The chapter with given index does not exist.");
        }

        if (await _context.Chapters.AnyAsync(
                x => x.BookId == request.BookId
                     && x.Index != request.CurrentIndex
                     && x.Index == request.NewIndex))
        {
            throw new ChapterAlreadyExistsException("The chapter with given index already exists");
        }

        chapter.Index = request.NewIndex;
        chapter.Content = request.Content;
        chapter.WordCount = request.WordCount;
        chapter.Name = request.Name;

        await _context.SaveChangesAsync();

        return new UpdateBookChapterResponse(
            await MapTotallyChapterToChapterVmAsync(chapter));
    }

    public async Task UpdateNextChapterIndexAsync(UpdateNextChapterIndexRequest request)
    {
        var book = await _context.Books.FindAsync(request.BookId);
        if (book is null)
        {
            throw new BookNotFoundException("The book with given Id does not exist.");
        }

        var currentChapter = await _context.Chapters
            .FirstOrDefaultAsync(x => x.BookId == request.BookId && x.Index == request.CurrentIndex);
        if (currentChapter is null)
        {
            throw new ChapterNotFoundException("The book's chapter with given index does not exist.");
        }
        
        var rel = await _context.ChapterRelationships
            .FirstOrDefaultAsync(x => x.FromId == currentChapter.Id);

        if (request.NextIndex is not null)
        {
            if (request.CurrentIndex == request.NextIndex)
            {
                throw new ArgumentException("CurrentIndex must not be equal to NextIndex.");
            }
            
            var nextChapter = await _context.Chapters
                .FirstOrDefaultAsync(x => x.BookId == request.BookId && x.Index == request.NextIndex);
            if (nextChapter is null)
            {
                throw new ChapterNotFoundException("The book's chapter with given index does not exist.");
            }
            
            if (rel is not null)
            {
                _context.ChapterRelationships.Remove(rel);
            }
            
            rel = new ChapterRelationship
            {
                FromId = currentChapter.Id,
                ToId = nextChapter.Id
            };

            await _context.ChapterRelationships.AddAsync(rel);

            await _context.SaveChangesAsync();
            return;
        }
        
        // If no specifying next chapter, then deleting the current rel
        if (rel is not null)
        {
            _context.ChapterRelationships.Remove(rel);
            await _context.SaveChangesAsync();
        }
    }

    public async Task UpdatePreviousChapterIndexAsync(UpdatePreviousChapterIndexRequest request)
    {
        var book = await _context.Books.FindAsync(request.BookId);
        if (book is null)
        {
            throw new BookNotFoundException("The book with given Id does not exist.");
        }

        var currentChapter = await _context.Chapters
            .FirstOrDefaultAsync(x => x.BookId == request.BookId && x.Index == request.CurrentIndex);
        if (currentChapter is null)
        {
            throw new ChapterNotFoundException("The book's chapter with given index does not exist.");
        }
        
        var rel = await _context.ChapterRelationships
            .FirstOrDefaultAsync(x => x.ToId == currentChapter.Id);

        if (request.PreviousIndex is not null)
        {
            if (request.CurrentIndex == request.PreviousIndex)
            {
                throw new ArgumentException("CurrentIndex must not be equal to PreviousIndex.");
            }
            
            var previousChapter = await _context.Chapters
                .FirstOrDefaultAsync(x => x.BookId == request.BookId && x.Index == request.PreviousIndex);
            if (previousChapter is null)
            {
                throw new ChapterNotFoundException("The book's chapter with given index does not exist.");
            }
            
            if (rel is not null)
            {
                rel.FromId = previousChapter.Id;
            }
            
            rel = new ChapterRelationship
            {
                FromId = previousChapter.Id,
                ToId = currentChapter.Id
            };

            await _context.ChapterRelationships.AddAsync(rel);

            await _context.SaveChangesAsync();
            return;
        }
        
        // If no specifying next chapter, then deleting the current rel
        if (rel is not null)
        {
            _context.ChapterRelationships.Remove(rel);
            await _context.SaveChangesAsync();
        }
    }

    private async Task<ChapterVm> MapTotallyChapterToChapterVmAsync(Chapter chapter)
    {
        var nextRel = await _context.ChapterRelationships
            .FirstOrDefaultAsync(x => x.FromId == chapter.Id);
        int? nextIndex = null;
        if (nextRel is not null)
        {
            var nextChapter = await _context.Chapters.FirstAsync(x => x.Id == nextRel.ToId);
            nextIndex = nextChapter.Index;
        }
        
        var prevRel = await _context.ChapterRelationships
            .FirstOrDefaultAsync(x => x.ToId == chapter.Id);
        int? prevIndex = null;
        if (prevRel is not null)
        {
            var prevChapter = await _context.Chapters.FirstAsync(x => x.Id == prevRel.FromId);
            prevIndex = prevChapter.Index;
        }
        
        var chapterVm = new ChapterVm(
            chapter.Id,
            chapter.Index,
            chapter.Name,
            chapter.CreatedAt,
            chapter.UpdatedAt,
            chapter.ReadCount,
            chapter.WordCount,
            chapter.Content,
            nextIndex,
            prevIndex,
            chapter.BookId
        );

        return chapterVm;
    }

    private ChapterVm MapChapterToChapterVm(Chapter chapter)
    {
        var chapterVm = new ChapterVm(
            chapter.Id,
            chapter.Index,
            chapter.Name,
            chapter.CreatedAt,
            chapter.UpdatedAt,
            chapter.ReadCount,
            chapter.WordCount,
            chapter.Content,
            null,
            null,
            chapter.BookId
        );

        return chapterVm;
    }
}
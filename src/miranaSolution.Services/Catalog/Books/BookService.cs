using Microsoft.EntityFrameworkCore;
using miranaSolution.Data.Entities;
using miranaSolution.Data.Main;
using miranaSolution.DTOs.Catalog.Books;
using miranaSolution.DTOs.Common;
using miranaSolution.Services.Exceptions;
using miranaSolution.Services.Systems.Files;

namespace miranaSolution.Services.Catalog.Books;

public class BookService : IBookService
{
    private readonly MiranaDbContext _context;
    private readonly IFileService _fileService;
    //
    public BookService(MiranaDbContext context, IFileService fileService)
    {
        _context = context;
        _fileService = fileService;
    }

    private async Task<string> SaveFileAsync(Stream stream, string fileExtension)
    {
        var newName = $"{Guid.NewGuid().ToString()}{fileExtension}";
        await _fileService.SaveFileAsync(
            stream, newName);

        return _fileService.GetPath(newName);
    }
    
    private bool IsValidExtension(string fileExtension)
    {
        var allowedExt = new List<string>() { ".jpg", ".jpeg", ".png" };
        return allowedExt.Contains(fileExtension);
    }

    public async Task<GetBookByIdResponse> GetBookByIdAsync(GetBookByIdRequest request)
    {
        var book = await _context.Books.FindAsync(request.BookId);
        if (book is null)
        {
            return new GetBookByIdResponse(null);
        }

        var bookVm = MapBookIntoBookVm(book);

        var response = new GetBookByIdResponse(bookVm);
        
        return response;
    }

    public async Task<GetBookBySlugResponse> GetBookBySlugAsync(GetBookBySlugRequest request)
    {
        var book = await _context.Books.FirstOrDefaultAsync(x => x.Slug.Equals(request.Slug));
        if (book is null)
        {
            return new GetBookBySlugResponse(null);
        }

        var bookVm = MapBookIntoBookVm(book);

        var response = new GetBookBySlugResponse(bookVm);
        
        return response;
    }
    
    /// <exception cref="BookAlreadyExistsException">
    /// Thrown when the book with given Slug already exists
    /// </exception>
    /// <exception cref="InvalidImageExtensionException">
    /// Thrown when the extension of thumbnail image is not allowed
    /// </exception>
    public async Task<CreateBookResponse> CreateBookAsync(CreateBookRequest request)
    {
        // TODO: Use FindUserById here

        var book = await _context.Books.FirstOrDefaultAsync(x => x.Slug.Equals(request.Slug));
        if (book is not null)
        {
            throw new BookAlreadyExistsException("The book with given Slug already exists.");
        }

        if (!IsValidExtension(request.ThumbnailImageExtension))
        {
            throw new InvalidImageExtensionException(
                "Invalid thumbnail image's extension. Only allow .jpg, .png and .jpeg.");
        }
        
        book = new Book
        {
            Name = request.Name,
            ShortDescription = request.ShortDescription,
            LongDescription = request.LongDescription,
            IsRecommended = request.IsRecommended,
            Slug = request.Slug,
            IsDone = request.IsDone,
            ThumbnailImage = 
                await SaveFileAsync(request.ThumbnailImage, request.ThumbnailImageExtension),
            UserId = request.UserId,
            AuthorId = request.AuthorId
        };

        await _context.Books.AddAsync(book);
        await _context.SaveChangesAsync();

        var bookVm = MapBookIntoBookVm(book);
        
        var response = new CreateBookResponse(bookVm);

        return response;
    }
    
    /// <exception cref="BookNotFoundException">
    /// Thrown when the book with given Id is not found
    /// </exception>
    public async Task<UpdateBookResponse> UpdateBookAsync(UpdateBookRequest request)
    {
        var book = await _context.Books.FindAsync(request.BookId);
        if (book is null)
        {
            throw new BookNotFoundException("The book with given Id does not exists.");
        }
        
        book.Name = request.Name;
        book.ShortDescription = request.ShortDescription;
        book.LongDescription = request.LongDescription;
        book.IsRecommended = request.IsRecommended;
        book.Slug = request.Slug;
        book.AuthorId = request.AuthorId;
        book.IsDone = request.IsDone;
        
        foreach (var item in request.GenreCheckboxItems)
            if (!item.IsChecked)
            {
                var bookGenre = await _context.BookGenres
                    .FirstOrDefaultAsync(x => x.BookId == book.Id && x.GenreId == item.Id);
                if (bookGenre is not null)
                {
                    _context.BookGenres.Remove(bookGenre);
                }
            }
            else
            {
                var bookGenre = new BookGenre()
                {
                    BookId = book.Id,
                    GenreId = item.Id
                };
        
                await _context.BookGenres.AddAsync(bookGenre);
            }
        
        await _context.SaveChangesAsync();

        var bookVm = MapBookIntoBookVm(book);
        var response = new UpdateBookResponse(bookVm);

        return response;
    }

    /// <exception cref="BookNotFoundException">
    /// Thrown when the book with given Id is not found
    /// </exception>
    public async Task DeleteBookAsync(DeleteBookRequest request)
    {
        var book = await _context.Books.FindAsync(request.BookId);
        if (book is null)
        {
            throw new BookNotFoundException("The book with given Id does not exist.");
        }
        
        _context.Books.Remove(book);
        await _context.SaveChangesAsync();
    }

    public async Task<GetRecommendedBooksResponse> GetRecommendedBooksAsync()
    {
        var books = await _context.Books
            .Where(x => x.IsRecommended)
            .ToListAsync();

        var bookVms = books.Select(MapBookIntoBookVm).ToList();
        
        return new GetRecommendedBooksResponse(bookVms);
    }

    public async Task<GetAllBooksResponse> GetAllBooksAsync(GetAllBooksRequest request)
    {
        var query = _context.Books.AsQueryable();
        
        // Apply books filter by keyword
        if (!string.IsNullOrEmpty(request.Keyword))
        {
            query = query.Where(x =>
                x.Name.Contains(request.Keyword)
                || x.ShortDescription.Contains(request.Keyword)
                || x.LongDescription.Contains(request.Keyword));
        }

        // Apply books filter by genre
        if (!string.IsNullOrEmpty(request.GenreIds))
        {
            var genreIds = request.GenreIds.Split(",").Select(int.Parse);
            foreach (var genreId in genreIds)
            {
                query = query
                    .Include(x => x.BookGenres)
                    .Where(x => x.BookGenres.Select(_ => _.GenreId).Contains(genreId));
            }
        }
        
        // Apply books filter by status
        if (request.IsDone.HasValue)
        {
            query = query.Where(x => x.IsDone == request.IsDone);
        }
        
        var pageSize = request.PagerRequest.PageSize;
        var pageIndex = request.PagerRequest.PageIndex;
        
        var totalBooks = await query.CountAsync();
        
        var books = await query
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        
        var bookVms = books.Select(MapBookIntoBookVm).ToList();

        var pagerResponse = new PagerResponse(
            request.PagerRequest.PageIndex,
            request.PagerRequest.PageSize,
            totalBooks);

        var response = new GetAllBooksResponse(
            bookVms,
            pagerResponse);
        
        return response;
    }

    private BookVm MapBookIntoBookVm(Book book)
    {
        _context.Attach(book);
        _context.Entry(book)
            .Reference(x => x.Author)
            .Load();
        _context.Entry(book)
            .Collection(x => x.BookGenres)
            .Load();

        var authorName = book.Author!.Name;
        var genres = book.BookGenres
            .Select(bookGenre => 
            {
                _context.Attach(bookGenre);
                _context.Entry(bookGenre)
                    .Reference(x => x.Genre)
                    .Load();

                return bookGenre.Genre!.Name;
            }).ToList();

        var bookVm = new BookVm(
            book.Id,
            book.Name,
            book.ShortDescription,
            book.LongDescription,
            book.CreatedAt,
            book.UpdatedAt,
            book.ThumbnailImage,
            book.IsRecommended,
            book.Slug,
            authorName,
            genres,
            book.IsDone);

        return bookVm;
    }
}
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using miranaSolution.Data.Entities;
using miranaSolution.Data.Main;
using miranaSolution.Dtos.Catalog.Books;
using miranaSolution.Dtos.Common;

namespace miranaSolution.Business.Catalog.Books
{
    public class BookService : IBookService
    {
        private readonly MiranaDbContext _context;
        private readonly IMapper _bookDtoMapper;

        public BookService(MiranaDbContext context)
        {
            _context = context;

            var config = new MapperConfiguration(cfg => cfg.CreateMap<Book, BookDto>());
            _bookDtoMapper = config.CreateMapper();
        }

        public async Task<BookDto> Create(BookCreateRequest request)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<BookCreateRequest, Book>());
            var mapper = config.CreateMapper();

            var newBook = mapper.Map<Book>(request);

            newBook.BookGenres = new List<BookGenre>();

            foreach (var item in request.Genres)
            {
                if (!item.IsChecked)
                    continue;

                var genre = await _context.Genres.FirstOrDefaultAsync(x => x.Id == item.Id && x.Name == item.Label);
                var bookGenre = new BookGenre
                {
                    Genre = genre
                };

                newBook.BookGenres.Add(bookGenre);
            }

            await _context.Books.AddAsync(newBook);
            await _context.SaveChangesAsync();

            var bookDto = _bookDtoMapper.Map<BookDto>(newBook);

            return bookDto;
        }

        public async Task<List<ChapterDto>> GetLatestChapters(int numOfChapters)
        {
            var query = from chapters in _context.Chapters
                join books in _context.Books on chapters.BookId equals books.Id
                join authors in _context.Authors on books.AuthorId equals authors.Id
                orderby chapters.CreatedAt descending
                select
                    new
                    {
                        chapters,
                        books,
                        authors,
                        genres = (from genres in _context.Genres
                            join bookGenres in _context.BookGenres on genres.Id equals bookGenres.GenreId
                            where bookGenres.BookId == books.Id
                            select genres.Name).ToList(),
                    };

            var data = await query.Take(numOfChapters).Select(x => new ChapterDto
            {
                Id = x.chapters.Id,
                Index = x.chapters.Index,
                Name = x.chapters.Name,
                CreatedAt = x.chapters.CreatedAt,
                UpdatedAt = x.chapters.UpdatedAt,
                ReadCount = x.chapters.ReadCount,
                WordCount = x.chapters.WordCount,
                Content = x.chapters.Content,
                BookId = x.books.Id,
                BookName = x.books.Name,
                BookAuthor = x.authors.Name,
                Genre = x.genres[0]
            }).ToListAsync();

            return data;
        }

        public async Task<bool> Delete(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book is null)
            {
                return false;
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<BookDto> GetById(int id)
        {
            var book = await _context.Books.FindAsync(id);
            var returnData = _bookDtoMapper.Map<BookDto>(book);

            return returnData;
        }

        public async Task<BookDto> GetBySlug(string slug)
        {
            var book = await _context.Books.FirstOrDefaultAsync(x => x.Slug == slug);
            var returnData = _bookDtoMapper.Map<BookDto>(book);

            return returnData;
        }

        public Task<ChapterDto> AddChapter(int id, ChapterCreateRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResult<BookDto>> GetPaging(BookGetPagingRequest request)
        {
            var query = _context.Books.AsQueryable();

            if (request.Keyword is not null)
            {
                query = query.Where(x =>
                    x.Name.Contains(request.Keyword) || x.ShortDescription.Contains(request.Keyword) ||
                    x.LongDescription.Contains(request.Keyword));
            }

            var pageSize = request.PageSize;
            var pageIndex = request.PageIndex;

            var totalRecords = await query.CountAsync();

            var data = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).Select(x => new BookDto
            {
                Id = x.Id,
                Name = x.Name,
                ShortDescription = x.ShortDescription,
                LongDescription = x.LongDescription,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                ThumbnailImage = x.ThumbnailImage,
                IsRecommended = x.IsRecommended,
                Slug = x.Slug
            }).ToListAsync();

            var paged = new PagedResult<BookDto>
            {
                TotalRecords = totalRecords,
                PageIndex = pageIndex,
                PageSize = pageSize,
                Items = data
            };

            return paged;
        }

        public async Task<List<BookDto>> GetRecommended()
        {
            var data = await _context.Books
                .Where(x => x.IsRecommended)
                .Select(x => _bookDtoMapper.Map<BookDto>(x))
                .ToListAsync();

            return data;
        }

        public async Task<bool> Update(int id, BookUpdateRequest request)
        {
            var book = await _context.Books.FindAsync(id);
            if (book is null)
            {
                return false;
            }

            book.Name = request.Name;
            book.ShortDescription = request.ShortDescription;
            book.LongDescription = request.LongDescription;
            book.IsRecommended = request.IsRecommended;
            book.Slug = request.Slug;
            book.AuthorId = request.AuthorId;

            foreach (var item in request.Genres)
            {
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
            }

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
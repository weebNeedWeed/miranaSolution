using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using miranaSolution.Business.Systems.Files;
using miranaSolution.Data.Entities;
using miranaSolution.Data.Main;
using miranaSolution.Dtos.Catalog.Books;
using miranaSolution.Dtos.Common;
using miranaSolution.Utilities.Exceptions;

namespace miranaSolution.Business.Catalog.Books
{
    public class BookService : IBookService
    {
        private readonly MiranaDbContext _context;
        private readonly IFileService _fileService;

        public BookService(MiranaDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        public async Task<BookDto> Create(BookCreateRequest request)
        {
            var config = new MapperConfiguration(cfg => { cfg.CreateMap<BookCreateRequest, Book>(); });
            var mapper = config.CreateMapper();

            var newBook = mapper.Map<Book>(request);
            newBook.BookGenres = new List<BookGenre>();
            newBook.ThumbnailImage = await this.SaveFileAsync(request.ThumnailImage);

            foreach (var item in request.Genres.Where(x => x.IsChecked))
            {
                var genre = await _context.Genres.FirstOrDefaultAsync(x => x.Id == item.Id && x.Name == item.Label);
                var bookGenre = new BookGenre
                {
                    GenreId = genre!.Id
                };

                newBook.BookGenres.Add(bookGenre);
            }

            await _context.Books.AddAsync(newBook);
            await _context.SaveChangesAsync();

            var bookDto = await this.GetById(newBook.Id);

            return bookDto;
        }

        public async Task<List<ChapterDto>> GetLatestChapters(int numOfChapters)
        {
            var query = from chapters in _context.Chapters
                join books in _context.Books on chapters.BookId equals books.Id
                orderby chapters.CreatedAt descending
                select
                    new
                    {
                        chapters,
                        books,
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
            var query = from book in _context.Books
                join book_author in _context.Authors
                    on book.AuthorId equals book_author.Id
                where book.Id == id
                select new
                {
                    book,
                    authorName = book_author.Name,
                    genres = (
                        from book_genre in _context.BookGenres
                        join genre in _context.Genres
                            on book_genre.GenreId equals genre.Id
                        where book_genre.BookId == id
                        select genre.Name).ToList()
                };
            var data = await query.FirstAsync();

            var config = new MapperConfiguration(cfg => cfg.CreateMap<Book, BookDto>());
            var mapper = config.CreateMapper();

            var bookDto = mapper.Map<BookDto>(data.book);
            bookDto.Genres = data.genres;
            bookDto.AuthorName = data.authorName;

            return bookDto;
        }

        public async Task<BookDto> GetBySlug(string slug)
        {
            var book = await _context.Books.FirstOrDefaultAsync(x => x.Slug == slug);
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Book, BookDto>());
            var mapper = config.CreateMapper();

            var returnData = mapper.Map<BookDto>(book);

            return returnData;
        }

        public async Task<ChapterDto> AddChapter(int id, ChapterCreateRequest request)
        {
            var isChapterExisted = await _context.Chapters.AnyAsync(x => x.BookId == id && x.Index == request.Index);
            if (isChapterExisted)
            {
                throw new MiranaBusinessException(
                    $"The book with Id: {id} already has a chapter with Index: {request.Index}");
            }

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Chapter, ChapterDto>();
                cfg.CreateMap<ChapterCreateRequest, Chapter>();
            });
            var mapper = config.CreateMapper();

            var chapter = mapper.Map<Chapter>(request);
            chapter.ReadCount = 0;
            chapter.BookId = id;

            await _context.Chapters.AddAsync(chapter);
            await _context.SaveChangesAsync();

            var chapterDto = mapper.Map<ChapterDto>(chapter);

            return chapterDto;
        }

        public async Task<PagedResult<BookDto>> GetPaging(BookGetPagingRequest request)
        {
            var query = from book in _context.Books
                join book_author in _context.Authors
                    on book.AuthorId equals book_author.Id
                select new
                {
                    book,
                    authorName = book_author.Name,
                    genres = (
                        from book_genre in _context.BookGenres
                        join genre in _context.Genres
                            on book_genre.GenreId equals genre.Id
                        where book_genre.BookId == book.Id
                        select genre.Name).ToList()
                };

            if (request.Keyword is not null)
            {
                query = query.Where(x =>
                    x.book.Name.Contains(request.Keyword) || x.book.ShortDescription.Contains(request.Keyword) ||
                    x.book.LongDescription.Contains(request.Keyword));
            }

            var pageSize = request.PageSize;
            var pageIndex = request.PageIndex;

            var totalRecords = await query.CountAsync();

            var data = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).Select(x => new BookDto
            {
                Id = x.book.Id,
                Name = x.book.Name,
                ShortDescription = x.book.ShortDescription,
                LongDescription = x.book.LongDescription,
                CreatedAt = x.book.CreatedAt,
                UpdatedAt = x.book.UpdatedAt,
                ThumbnailImage = x.book.ThumbnailImage,
                IsRecommended = x.book.IsRecommended,
                Slug = x.book.Slug,
                AuthorName = x.authorName,
                Genres = x.genres
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
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Book, BookDto>());
            var mapper = config.CreateMapper();

            var data = await _context.Books
                .Where(x => x.IsRecommended)
                .Select(x => mapper.Map<BookDto>(x))
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

        private async Task<string> SaveFileAsync(IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName);
            var newName = $"{Guid.NewGuid().ToString()}{extension}";

            return await _fileService.SaveFileAsync(
                file.OpenReadStream(), newName);
        }
    }
}
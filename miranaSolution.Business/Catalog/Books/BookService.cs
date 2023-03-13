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
                if (item.IsChecked)
                {
                    var genre = await _context.Genres.FirstOrDefaultAsync(x => x.Id == item.Id && x.Name == item.Label);
                    var bookGenre = new BookGenre
                    {
                        Genre = genre
                    };

                    newBook.BookGenres.Add(bookGenre);
                }
            }

            await _context.Books.AddAsync(newBook);
            await _context.SaveChangesAsync();

            var bookDto = _bookDtoMapper.Map<BookDto>(newBook);

            return bookDto;
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
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

        public async Task<PagedResult<BookDto>> GetPaging(BookGetPagingRequest request)
        {
            var query = _context.Books.AsQueryable();

            if (request.Keyword is not null)
            {
                query = query.Where(x => x.Name.Contains(request.Keyword) || x.ShortDescription.Contains(request.Keyword) || x.LongDescription.Contains(request.Keyword));
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

        public Task<bool> Update(int id, BookUpdateRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
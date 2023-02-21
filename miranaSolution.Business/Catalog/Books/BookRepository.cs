using Microsoft.EntityFrameworkCore;
using miranaSolution.Data.Main;
using miranaSolution.Dtos.Catalog.Books;
using miranaSolution.Dtos.Common;

namespace miranaSolution.Business.Catalog.Books
{
    public class BookRepository : IBookRepository
    {
        private readonly MiranaDbContext _context;

        public BookRepository(MiranaDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<BookDto>> GetPagingAsync(BookGetPagingRequest request)
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
    }
}
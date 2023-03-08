using Microsoft.AspNetCore.Mvc;
using miranaSolution.Business.Catalog.Books;
using miranaSolution.Data.Entities;
using miranaSolution.Dtos.Catalog.Books;
using miranaSolution.Dtos.Common;

namespace miranaSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPaging([FromQuery] BookGetPagingRequest request)
        {
            var books = await _bookService.GetPaging(request);

            return Ok(new ApiSuccessResult<PagedResult<BookDto>>(books));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] BookCreateRequest request)
        {
            var errors = new Dictionary<string, List<string>>();

            var isBookWithSlugExisted = (await _bookService.GetBySlug(request.Slug)) is not null;
            if (isBookWithSlugExisted)
            {
                errors.Add(nameof(request.Slug), new List<string> { "Duplicated slug." });
                return Ok(new ApiFailResult(errors));
            }

            var book = await _bookService.Create(request);

            return Ok(new ApiSuccessResult<BookDto>(book));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var book = await _bookService.GetById(id);
            if (book is null)
            {
                return Ok(new ApiFailResult(new Dictionary<string, List<string>> {
                        { nameof(book.Id), new List<string> { $"Invalid Id." } }
                    }));
            }

            return Ok(new ApiSuccessResult<BookDto>(book));
        }

        [HttpGet("recommended")]
        public async Task<IActionResult> GetRecommended()
        {
            var books = await _bookService.GetRecommended();

            return Ok(new ApiSuccessResult<List<BookDto>>(books));
        }
    }
}
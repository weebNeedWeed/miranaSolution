using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using miranaSolution.Business.Catalog.Books;
using miranaSolution.Dtos.Catalog.Books;
using miranaSolution.Dtos.Common;
using miranaSolution.Utilities.Exceptions;

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

            return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            BookDto book;

            try
            {
                book = await _bookService.GetById(id);
            }
            catch (MiranaBusinessException ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(book);
        }

        [HttpGet("recommended")]
        public async Task<IActionResult> GetRecommended()
        {
            var books = await _bookService.GetRecommended();

            return Ok(books);
        }
    }
}
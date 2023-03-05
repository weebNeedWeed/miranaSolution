using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using miranaSolution.Business.Catalog.Books;
using miranaSolution.Dtos.Catalog.Books;
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

            return Ok(books);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] BookCreateRequest request)
        {
            BookDto book;

            try
            {
                book = await _bookService.Create(request);
            }
            catch (MiranaBusinessException ex)
            {
                return BadRequest(ex.Message);
            }

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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using miranaSolution.Business.Catalog.Books;
using miranaSolution.Dtos.Catalog.Books;

namespace miranaSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;

        public BooksController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetPaging([FromQuery] BookGetPagingRequest request)
        {
            var books = await _bookRepository.GetPagingAsync(request);

            return Ok(books);
        }
    }
}
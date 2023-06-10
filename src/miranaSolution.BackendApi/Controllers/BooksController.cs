using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using miranaSolution.Business.Catalog.Books;
using miranaSolution.Business.Systems.Files;
using miranaSolution.Dtos.Catalog.Books;
using miranaSolution.Dtos.Catalog.Books.Chapters;
using miranaSolution.Dtos.Common;
using miranaSolution.Utilities.Constants;
using miranaSolution.Utilities.Exceptions;

namespace miranaSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = RolesConstant.Administrator)]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService, IFileService fileService)
        {
            _bookService = bookService;
        }

        // GET /api/books
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetPaging([FromQuery] BookGetPagingRequest request)
        {
            var books = await _bookService.GetPaging(request);

            return Ok(new ApiSuccessResult<PagedResult<BookDto>>(books));
        }

        // POST /api/books
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] BookCreateRequest request)
        {
            var errors = new Dictionary<string, List<string>>();

            if (!HasValidExtension(request.ThumbnailImage.FileName))
            {
                errors.Add(nameof(request.ThumbnailImage), new List<string> { "Invalid image extension." });
                return Ok(new ApiFailResult(errors));
            }

            var isBookWithSlugExisted = (await _bookService.GetBySlug(request.Slug)) is not null;
            if (isBookWithSlugExisted)
            {
                errors.Add(nameof(request.Slug), new List<string> { "Duplicated slug." });
                return Ok(new ApiFailResult(errors));
            }

            var userId = User.Claims.First(x => x.Type == "sid").Value;

            var book = await _bookService.Create(userId, request);

            return Ok(new ApiSuccessResult<BookDto>(book));
        }

        // GET /api/books/{id}
        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var book = await _bookService.GetById(id);
            if (book is null)
            {
                return Ok(new ApiFailResult(new Dictionary<string, List<string>>
                {
                    { nameof(book.Id), new List<string> { $"Invalid Id." } }
                }));
            }

            return Ok(new ApiSuccessResult<BookDto>(book));
        }

        // GET /api/books/recommended
        [HttpGet("recommended")]
        [AllowAnonymous]
        public async Task<IActionResult> GetRecommended()
        {
            var books = await _bookService.GetRecommended();
            return Ok(new ApiSuccessResult<List<BookDto>>(books));
        }

        // GET /api/books/chapters/latest/{numOfChapters}
        [HttpGet("chapters/latest/{numOfChapters:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetLatestChapter([FromRoute] int numOfChapters)
        {
            var chapters = await _bookService.GetLatestChapters(numOfChapters);
            return Ok(new ApiSuccessResult<List<ChapterDto>>(chapters));
        }

        // POST /api/books/{id}/chapters
        [HttpPost("{id:int}/chapters")]
        [AllowAnonymous]
        public async Task<IActionResult> AddChapter([FromRoute] int id, [FromBody] ChapterCreateRequest request)
        {
            try
            {
                var chapter = await _bookService.AddChapter(id, request);

                return Ok(new ApiSuccessResult<ChapterDto>(chapter));
            }
            catch (MiranaBusinessException exception)
            {
                return BadRequest(new ApiErrorResult(exception.Message));
            }
        }

        // GET /api/books/{id}/chapters
        [HttpGet("{id:int}/chapters")]
        [AllowAnonymous]
        public async Task<IActionResult> GetChaptersPaging([FromRoute] int id, [FromQuery] ChapterGetPagingRequest request)
        {
            var chaptersPaging = await _bookService.GetChaptersPaging(id, request);
            return Ok(new ApiSuccessResult<PagedResult<ChapterDto>>(chaptersPaging));
        }

        private bool HasValidExtension(string fileName)
        {
            var allowedExt = new List<string>() { ".jpg", ".jpeg", ".png" };
            return allowedExt.Contains(Path.GetExtension(fileName));
        }
    }
}
﻿using Microsoft.AspNetCore.Mvc;
using miranaSolution.Business.Catalog.Books;
using miranaSolution.Business.Systems.Files;
using miranaSolution.Dtos.Catalog.Books;
using miranaSolution.Dtos.Common;

namespace miranaSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService, IFileService fileService)
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
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] BookCreateRequest request)
        {
            var errors = new Dictionary<string, List<string>>();

            if (!HasValidExtension(request.ThumnailImage.FileName))
            {
                errors.Add(nameof(request.ThumnailImage), new List<string> { "Invalid image extension." });
                return Ok(new ApiFailResult(errors));
            }

            var isBookWithSlugExisted = (await _bookService.GetBySlug(request.Slug)) is not null;
            if (isBookWithSlugExisted)
            {
                errors.Add(nameof(request.Slug), new List<string> { "Duplicated slug." });
                return Ok(new ApiFailResult(errors));
            }

            var book = await _bookService.Create(request);

            return Ok(new ApiSuccessResult<BookDto>(book));
        }

        [HttpGet("{id:int}")]
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

        [HttpGet("recommended")]
        public async Task<IActionResult> GetRecommended()
        {
            var books = await _bookService.GetRecommended();
            return Ok(new ApiSuccessResult<List<BookDto>>(books));
        }

        [HttpGet("chapters/latest/{numOfChapters:int}")]
        public async Task<IActionResult> GetLatestChapter([FromRoute] int numOfChapters)
        {
            var chapters = await _bookService.GetLatestChapters(numOfChapters);
            return Ok(new ApiSuccessResult<List<ChapterDto>>(chapters));
        }

        [HttpPost("{id:int}/chapters")]
        public async Task<IActionResult> AddChapter([FromRoute] int id)
        {
            return Ok();
        }

        private bool HasValidExtension(string fileName)
        {
            var allowedExt = new List<string>() { ".jpg", ".jpeg", ".png" };
            return allowedExt.Contains(Path.GetExtension(fileName));
        }
    }
}
using miranaSolution.DTOs.Catalog.Books.Chapters;
using miranaSolution.DTOs.Common;
using Refit;

namespace miranaSolution.Admin.Services.Interfaces;

public interface IBooksApiService
{
    [Get("/books")]
    Task<ApiResult<PagedResult<BookDto>>> GetPaging(BookGetPagingRequest request);

    [Multipart]
    [Post("/books")]
    Task<ApiResult<dynamic>> Create([AliasAs("Name")] string name,
        [AliasAs("ShortDescription")] string shortDescription,
        [AliasAs("LongDescription")] string longDescription,
        [AliasAs("IsRecommended")] bool isRecommended,
        [AliasAs("Slug")] string slug,
        [AliasAs("AuthorId")] int authorId,
        [AliasAs("ThumbnailImage")] ByteArrayPart thumbnailImage,
        [Header("Authorization")] string authorization);

    [Get("/books/{id}")]
    Task<ApiResult<BookDto>> GetById([AliasAs("id")] int id);

    [Get("/books/{id}/chapters")]
    Task<ApiResult<PagedResult<ChapterDto>>> GetChaptersPaging([AliasAs("id")] int id, ChapterGetPagingRequest request);

    [Post("/books/{id}/chapters")]
    Task<ApiResult<ChapterDto>> AddChapter([AliasAs("id")] int id, [Body] ChapterCreateRequest request);
}
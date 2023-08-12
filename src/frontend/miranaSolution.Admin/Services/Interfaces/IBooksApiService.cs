using miranaSolution.API.ViewModels.Books;
using miranaSolution.API.ViewModels.Common;
using miranaSolution.DTOs.Core.Books;
using Refit;

namespace miranaSolution.Admin.Services.Interfaces;

public interface IBooksApiService
{
    [Get("/api/books")]
    Task<ApiResult<ApiGetAllBooksResponse>> GetAllBooksAsync(ApiGetAllBooksRequest request);

    [Multipart]
    [Post("/api/books")]
    Task<ApiResult<dynamic>> CreateAsync([AliasAs("Name")] string name,
        [AliasAs("ShortDescription")] string shortDescription,
        [AliasAs("LongDescription")] string longDescription,
        [AliasAs("IsRecommended")] bool isRecommended,
        [AliasAs("Slug")] string slug,
        [AliasAs("AuthorId")] int authorId,
        [AliasAs("IsDone")] bool isDone,
        [AliasAs("ThumbnailImage")] ByteArrayPart thumbnailImage);

    [Get("/api/books/{id}")]
    Task<ApiResult<BookVm>> GetBookByIdAsync([AliasAs("id")] int id);

    [Get("/api/books/{id}/chapters")]
    Task<ApiResult<ApiGetAllBookChaptersResponse>> GetAllBookChaptersAsync([AliasAs("id")] int id, ApiGetAllBookChaptersRequest request);

    [Post("/api/books/{id}/chapters")]
    Task<ApiResult<dynamic>> CreateBookChapterAsync(
        [AliasAs("id")] int id, 
        [Body] ApiCreateBookChapterRequest request);
}
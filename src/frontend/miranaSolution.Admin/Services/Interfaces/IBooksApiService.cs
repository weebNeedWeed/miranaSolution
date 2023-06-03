using miranaSolution.Dtos.Catalog.Books;
using miranaSolution.Dtos.Common;
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
}
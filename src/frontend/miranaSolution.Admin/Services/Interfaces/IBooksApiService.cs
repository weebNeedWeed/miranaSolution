using miranaSolution.API.ViewModels.Books;
using miranaSolution.API.ViewModels.Common;
using miranaSolution.DTOs.Core.Books;
using miranaSolution.DTOs.Core.Chapters;
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
    Task<ApiResult<ApiGetAllBookChaptersResponse>> GetAllBookChaptersAsync(
        [AliasAs("id")] int id, 
        ApiGetAllBookChaptersRequest request);
    
    [Get("/api/books/{id}/chapters/{index}")]
    Task<ApiResult<ChapterVm>> GetBookChapterByIndexAsync(
        [AliasAs("id")] int id, 
        [AliasAs("index")] int index);
    
    [Put("/api/books/{id}/chapters/{index}/next/{nextIndex}")]
    Task<ApiResult<ChapterVm>> UpdateNextChapterIndex(
        [AliasAs("id")] int id, 
        [AliasAs("index")] int index,
        [AliasAs("nextIndex")] int nextIndex);
    
    [Put("/api/books/{id}/chapters/{index}/previous/{previousIndex}")]
    Task<ApiResult<ChapterVm>> UpdatePreviousChapterIndex(
        [AliasAs("id")] int id, 
        [AliasAs("index")] int index,
        [AliasAs("previousIndex")] int previousIndex);
    
    [Delete("/api/books/{id}/chapters/{index}/next")]
    Task<ApiResult<ChapterVm>> RemoveNextChapterIndex(
        [AliasAs("id")] int id, 
        [AliasAs("index")] int index);
    
    [Delete("/api/books/{id}/chapters/{index}/previous")]
    Task<ApiResult<ChapterVm>> RemovePreviousChapterIndex(
        [AliasAs("id")] int id, 
        [AliasAs("index")] int index);

    [Post("/api/books/{id}/chapters")]
    Task<ApiResult<dynamic>> CreateBookChapterAsync(
        [AliasAs("id")] int id, 
        [Body] ApiCreateBookChapterRequest request);
    
    [Delete("/api/books/{id}/chapters/{index}")]
    Task<ApiResult<dynamic>> DeleteBookChapterAsync(
        [AliasAs("id")] int id, 
        [AliasAs("index")] int index);
    
    [Put("/api/books/{id}/chapters/{index}")]
    Task<ApiResult<dynamic>> UpdateBookChapterAsync(
        [AliasAs("id")] int id, 
        [AliasAs("index")] int index, 
        [Body] ApiUpdateBookChapterRequest request);

    [Delete("/api/books/{id}")]
    Task<ApiResult<dynamic>> DeleteBookAsync(
        [AliasAs("id")] int id);
    
    [Get("/api/books/{id}/genres")]
    Task<ApiResult<ApiGetBookGenresByBookIdResponse>> GetAllGenresByBookIdAsync(
        [AliasAs("id")] int id);
    
    [Post("/api/books/{id}")]
    [Multipart]
    Task<ApiResult<dynamic>> UpdateBookAsync(
        [AliasAs("id")] int id,
        [AliasAs("name")] string Name,
        [AliasAs("shortDescription")]string ShortDescription,
        [AliasAs("longDescription")] string LongDescription,
        [AliasAs("isRecommended")] bool IsRecommended,
        [AliasAs("slug")] string Slug,
        [AliasAs("authorId")] int AuthorId,
        [AliasAs("isDone")] bool IsDone,
        [AliasAs("thumbnailImage")] StreamPart? ThumbnailImage);
}
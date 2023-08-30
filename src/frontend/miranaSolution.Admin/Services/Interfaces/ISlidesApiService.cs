using miranaSolution.API.ViewModels.Common;
using miranaSolution.API.ViewModels.Slides;
using miranaSolution.DTOs.Core.Slides;
using Refit;

namespace miranaSolution.Admin.Services.Interfaces;

public interface ISlidesApiService
{
    [Get("/api/slides")]
    Task<ApiResult<ApiGetAllSlidesResponse>> GetAllSlidesAsync();

    [Post("/api/slides")]
    [Multipart]
    Task<ApiResult<dynamic>> CreateSlideAsync(
        [AliasAs("name")] string name,
        [AliasAs("shortDescription")]string shortDescription,
        [AliasAs("genres")]string genres,
        [AliasAs("sortOrder")]int sortOrder,
        [AliasAs("url")]string url,
        [AliasAs("thumbnailImage")] StreamPart thumbnailImage);

    [Get("/api/slides/{id}")]
    Task<ApiResult<SlideVm>> GetSlideByIdAsync(
        [AliasAs("id")] int id);
    
    [Delete("/api/slides/{id}")]
    Task<ApiResult<dynamic>> DeleteSlideAsync(
        [AliasAs("id")] int id);

    [Post("/api/slides/{id}")]
    [Multipart]
    Task<ApiResult<dynamic>> UpdateSlideAsync(
        [AliasAs("id")] int id,
        [AliasAs("name")] string name,
        [AliasAs("shortDescription")] string shortDescription,
        [AliasAs("genres")] string genres,
        [AliasAs("sortOrder")] int sortOrder,
        [AliasAs("url")]string url,
        [AliasAs("thumbnailImage")] StreamPart? thumbnailImage);
}
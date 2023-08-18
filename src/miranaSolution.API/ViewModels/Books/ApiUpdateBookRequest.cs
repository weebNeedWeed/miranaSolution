using miranaSolution.DTOs.Common;

namespace miranaSolution.API.ViewModels.Books;

public record ApiUpdateBookRequest(
    string Name,
    string ShortDescription,
    string LongDescription,
    bool IsRecommended,
    string Slug,
    int AuthorId,
    bool IsDone,
    IFormFile? ThumbnailImage);
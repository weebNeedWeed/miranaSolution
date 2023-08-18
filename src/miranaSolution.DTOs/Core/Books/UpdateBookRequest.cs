using miranaSolution.DTOs.Common;

namespace miranaSolution.DTOs.Core.Books;

public record UpdateBookRequest(
    int BookId,
    string Name,
    string ShortDescription,
    string LongDescription,
    bool IsRecommended,
    string Slug,
    int AuthorId,
    bool IsDone,
    Stream? ThumbnailImage,
    string? ThumbnailImageExtension);
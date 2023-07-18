namespace miranaSolution.DTOs.Core.Books;

public record CreateBookRequest(
    string Name,
    string ShortDescription,
    string LongDescription,
    bool IsRecommended,
    bool IsDone,
    string Slug,
    int AuthorId, 
    Stream ThumbnailImage,
    string ThumbnailImageExtension);
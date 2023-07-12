namespace miranaSolution.API.ViewModels.Books;

public record ApiCreateBookRequest(
    string Name,
    string ShortDescription,
    string LongDescription,
    bool IsRecommended,
    bool IsDone,
    string Slug,
    int AuthorId, 
    IFormFile ThumbnailImage);
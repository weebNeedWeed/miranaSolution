namespace miranaSolution.API.ViewModels.Slides;

public record ApiCreateSlideRequest(
    string Name,
    string ShortDescription,
    string Genres,
    int SortOrder,
    string Url,
    IFormFile ThumbnailImage);

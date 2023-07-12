namespace miranaSolution.API.ViewModels.Slides;

public record ApiCreateSlideRequest(
    string Name,
    string ShortDescription,
    string ThumbnailImage,
    string Genres,
    int SortOrder);
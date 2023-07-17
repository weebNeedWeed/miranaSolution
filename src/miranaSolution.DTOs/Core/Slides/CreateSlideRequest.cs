namespace miranaSolution.DTOs.Core.Slides;

public record CreateSlideRequest(
    string Name,
    string ShortDescription,
    string ThumbnailImage,
    string Genres,
    int SortOrder);
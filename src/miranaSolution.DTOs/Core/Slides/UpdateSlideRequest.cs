namespace miranaSolution.DTOs.Core.Slides;

public record UpdateSlideRequest(
    int SlideId,
    string Name,
    string ShortDescription,
    string Genres,
    int SortOrder,
    string Url,
    Stream? ThumbnailImage,
    string? ThumbnailImageExtension);
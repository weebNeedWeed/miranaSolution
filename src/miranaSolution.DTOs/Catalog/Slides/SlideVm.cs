namespace miranaSolution.DTOs.Catalog.Slides;

public record SlideVm(
    int Id,
    string Name,
    string ShortDescription,
    string ThumbnailImage,
    string Genres,
    int SortOrder);
﻿namespace miranaSolution.API.ViewModels.Slides;

public record ApiUpdateSlideRequest(
    string Name,
    string ShortDescription,
    string Genres,
    int SortOrder,
    IFormFile? ThumbnailImage);
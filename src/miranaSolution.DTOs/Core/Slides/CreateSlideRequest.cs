﻿namespace miranaSolution.DTOs.Core.Slides;

public record CreateSlideRequest(
    string Name,
    string ShortDescription,
    string Genres,
    int SortOrder,
    Stream ThumbnailImage,
    string ThumbnailImageExtension);
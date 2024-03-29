﻿namespace miranaSolution.DTOs.Core.Books;

public record BookVm(
    int Id,
    string Name,
    string ShortDescription,
    string LongDescription,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    string ThumbnailImage,
    bool IsRecommended,
    string Slug,
    string AuthorName,
    List<string> Genres,
    bool IsDone,
    int AuthorId,
    int ViewCount);
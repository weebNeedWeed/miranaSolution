﻿using Microsoft.AspNetCore.Http;

namespace miranaSolution.DTOs.Catalog.Books;

public record CreateBookRequest(
    Guid UserId,
    string Name,
    string ShortDescription,
    string LongDescription,
    bool IsRecommended,
    bool IsDone,
    string Slug,
    int AuthorId, 
    Stream ThumbnailImage,
    string ThumbnailImageExtension);
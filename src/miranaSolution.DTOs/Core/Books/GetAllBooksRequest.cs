﻿using miranaSolution.DTOs.Common;

namespace miranaSolution.DTOs.Core.Books;

public record GetAllBooksRequest(
    string? Keyword,
    string? GenreIds,
    bool? IsDone,
    int? AuthorId,
    PagerRequest PagerRequest);
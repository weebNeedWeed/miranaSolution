﻿namespace miranaSolution.DTOs.Core.Genres;

public record CreateGenreRequest(
    string Name,
    string ShortDescription,
    string Slug);
﻿using miranaSolution.DTOs.Catalog.Chapters;

namespace miranaSolution.Services.Catalog.Chapters;

public interface IChapterService
{
    Task<CreateBookChapterResponse> CreateBookChapterAsync(CreateBookChapterRequest request);

    Task<GetAllBookChaptersResponse> GetAllBookChaptersAsync(GetAllBookChaptersRequest request);

    Task<GetBookChapterByIndexResponse> GetBookChapterByIndexAsync(GetBookChapterByIndexRequest request);

    Task<GetTotalBookChaptersResponse> GetTotalBookChaptersAsync(GetTotalBookChaptersRequest request);

    Task<GetLatestCreatedChaptersResponse> GetLatestCreatedChaptersAsync(GetLatestCreatedChaptersRequest request);
}
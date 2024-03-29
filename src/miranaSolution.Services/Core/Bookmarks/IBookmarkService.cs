﻿using miranaSolution.DTOs.Core.Bookmarks;

namespace miranaSolution.Services.Core.Bookmarks;

public interface IBookmarkService
{
    Task<CreateBookmarkResponse> CreateBookmarkAsync(CreateBookmarkRequest request);

    Task DeleteBookmarkAsync(DeleteBookmarkRequest request);

    Task<GetAllBookmarksByUserIdResponse> GetAllBookmarksByUserIdAsync(GetAllBookmarksByUserIdRequest request);

    Task<GetAllBookmarksByBookIdResponse> GetAllBookmarksByBookIdAsync(GetAllBookmarksByBookIdRequest request);
}
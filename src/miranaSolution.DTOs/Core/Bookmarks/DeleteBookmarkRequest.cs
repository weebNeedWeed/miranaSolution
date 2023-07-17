namespace miranaSolution.DTOs.Core.Bookmarks;

public record DeleteBookmarkRequest(
    Guid UserId,
    int BookId);
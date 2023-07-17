namespace miranaSolution.DTOs.Core.Bookmarks;

public record CreateBookmarkRequest(
    Guid UserId,
    int BookId,
    int ChapterIndex);
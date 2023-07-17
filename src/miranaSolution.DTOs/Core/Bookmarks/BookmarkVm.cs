namespace miranaSolution.DTOs.Core.Bookmarks;

public record BookmarkVm(
    Guid UserId,
    int BookId,
    int ChapterIndex,
    DateTime CreatedAt,
    DateTime UpdatedAt);
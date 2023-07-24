namespace miranaSolution.DTOs.Core.Bookmarks;

public record GetAllBookmarksByUserIdRequest(
    Guid UserId,
    int? BookId);
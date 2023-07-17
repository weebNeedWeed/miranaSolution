namespace miranaSolution.DTOs.Core.Bookmarks;

public record GetAllBookmarksByUserIdResponse(
    List<BookmarkVm> BookmarkVms);
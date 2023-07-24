using miranaSolution.DTOs.Core.Bookmarks;

namespace miranaSolution.API.ViewModels.Bookmarks;

public record ApiGetAllBookmarksResponse(List<BookmarkVm> Bookmarks);
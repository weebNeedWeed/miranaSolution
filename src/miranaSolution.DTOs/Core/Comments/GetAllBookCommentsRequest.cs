using miranaSolution.DTOs.Common;

namespace miranaSolution.DTOs.Core.Comments;

public record GetAllBookCommentsRequest(
    int BookId,
    int? ParentId,
    bool? Asc,
    PagerRequest PagerRequest);
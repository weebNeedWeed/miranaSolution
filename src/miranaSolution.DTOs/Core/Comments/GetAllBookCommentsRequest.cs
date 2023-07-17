using miranaSolution.DTOs.Common;

namespace miranaSolution.DTOs.Core.Comments;

public record GetAllBookCommentsRequest(
    int BookId,
    PagerRequest PagerRequest);
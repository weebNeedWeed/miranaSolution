using miranaSolution.DTOs.Common;

namespace miranaSolution.DTOs.Core.Comments;

public record GetAllBookCommentsResponse(
    List<CommentVm> CommentVms,
    PagerResponse PagerResponse);
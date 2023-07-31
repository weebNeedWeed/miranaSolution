using miranaSolution.DTOs.Core.Comments;

namespace miranaSolution.API.ViewModels.Books;

public record ApiGetAllBookCommentsResponse(
    List<CommentVm> Comments,
    int PageIndex,
    int PageSize,
    int TotalPages,
    int TotalComments);
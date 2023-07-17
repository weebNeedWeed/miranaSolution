namespace miranaSolution.DTOs.Core.Comments;

public record GetBookCommentByIdRequest(
    int CommentId,
    int BookId);
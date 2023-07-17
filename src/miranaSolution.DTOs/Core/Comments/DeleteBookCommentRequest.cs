namespace miranaSolution.DTOs.Core.Comments;

public record DeleteBookCommentRequest(
    int CommentId,
    int BookId,
    Guid UserId,
    bool ForceDelete);
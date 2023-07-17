namespace miranaSolution.DTOs.Core.Comments;

public record CreateBookCommentRequest(
    int BookId,
    Guid UserId,
    string Content,
    int? ParentId);
namespace miranaSolution.DTOs.Core.Comments;

public record CommentVm(
    int Id,
    string Content,
    int? ParentId,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    Guid? UserId,
    string Username,
    string UserAvatar,
    int BookId);
namespace miranaSolution.DTOs.Core.CommentReactions;

public record CreateCommentReactionRequest(
    Guid UserId,
    int CommentId);
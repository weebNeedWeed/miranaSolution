namespace miranaSolution.DTOs.Core.CommentReactions;

public record DeleteCommentReactionRequest(
    Guid UserId, 
    int CommentId);
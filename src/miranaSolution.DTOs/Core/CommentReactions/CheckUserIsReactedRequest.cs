namespace miranaSolution.DTOs.Core.CommentReactions;

public record CheckUserIsReactedRequest(
    Guid UserId,
    int CommentId);
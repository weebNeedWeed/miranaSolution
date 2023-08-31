namespace miranaSolution.DTOs.Core.BookRatings;

public record BookRatingVm(
    Guid UserId,
    string Username,
    string UserAvatar,
    int BookId,
    string Content,
    int Star,
    DateTime CreatedAt,
    DateTime UpdatedAt);
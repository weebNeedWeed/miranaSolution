namespace miranaSolution.DTOs.Core.BookRatings;

public record BookRatingVm(
    Guid UserId,
    int BookId,
    string Content, 
    int Star,
    DateTime CreatedAt,
    DateTime UpdatedAt);
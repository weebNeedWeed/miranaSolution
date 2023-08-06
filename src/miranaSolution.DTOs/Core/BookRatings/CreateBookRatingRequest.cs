namespace miranaSolution.DTOs.Core.BookRatings;

public record CreateBookRatingRequest(
    Guid UserId,
    int BookId,
    string Content,
    int Star);
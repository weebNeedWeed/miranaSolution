namespace miranaSolution.DTOs.Core.BookRatings;

public record DeleteBookRatingRequest(
    Guid UserId,
    int BookId);
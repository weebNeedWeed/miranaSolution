namespace miranaSolution.DTOs.Core.BookRatings;

public record UpdateBookRatingRequest(
    Guid UserId,
    int BookId,
    string Content, 
    int Star);
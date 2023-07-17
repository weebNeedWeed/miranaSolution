namespace miranaSolution.DTOs.Core.BookUpvotes;

public record DeleteBookUpvoteRequest(
    Guid UserId,
    int BookId);
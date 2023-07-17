namespace miranaSolution.DTOs.Core.BookUpvotes;

public record CreateBookUpvoteRequest(
    Guid UserId,
    int BookId);
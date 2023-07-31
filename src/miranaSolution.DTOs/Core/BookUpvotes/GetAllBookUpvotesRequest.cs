namespace miranaSolution.DTOs.Core.BookUpvotes;

public record GetAllBookUpvotesRequest(
    int BookId,
    Guid? UserId);
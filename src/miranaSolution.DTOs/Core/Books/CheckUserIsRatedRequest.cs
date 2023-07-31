namespace miranaSolution.DTOs.Core.Books;

public record CheckUserIsRatedRequest(
    Guid UserId,
    int BookId);
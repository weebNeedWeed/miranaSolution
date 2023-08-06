namespace miranaSolution.DTOs.Core.CurrentlyReading;

public record GetCurrentlyReadingBooksRequest(
    Guid UserId,
    int? BookId);
namespace miranaSolution.DTOs.Core.CurrentlyReading;

public record AddBookRequest(
    int BookId,
    Guid UserId,
    int ChapterIndex);
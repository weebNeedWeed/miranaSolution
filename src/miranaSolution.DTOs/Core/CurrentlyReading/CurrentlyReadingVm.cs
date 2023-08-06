namespace miranaSolution.DTOs.Core.CurrentlyReading;

public record CurrentlyReadingVm(
    int BookId,
    Guid UserId, 
    int ChapterIndex,
    DateTime CreatedAt);
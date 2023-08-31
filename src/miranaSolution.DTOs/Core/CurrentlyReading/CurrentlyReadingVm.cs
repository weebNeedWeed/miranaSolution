namespace miranaSolution.DTOs.Core.CurrentlyReading;

public record CurrentlyReadingVm(
    int BookId,
    string BookName,
    string ThumbnailImage,
    string BookSlug,
    Guid UserId, 
    int ChapterIndex,
    DateTime CreatedAt);
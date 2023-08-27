namespace miranaSolution.DTOs.Core.Chapters;

public record DeleteBookChapterRequest(
    int BookId,
    int Index);
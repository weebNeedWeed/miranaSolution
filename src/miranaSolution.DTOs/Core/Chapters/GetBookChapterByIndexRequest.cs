namespace miranaSolution.DTOs.Core.Chapters;

public record GetBookChapterByIndexRequest(
    int BookId,
    int ChapterIndex);
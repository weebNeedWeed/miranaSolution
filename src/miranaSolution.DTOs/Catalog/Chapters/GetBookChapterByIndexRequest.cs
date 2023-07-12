namespace miranaSolution.DTOs.Catalog.Chapters;

public record GetBookChapterByIndexRequest(
    int BookId,
    int ChapterIndex);
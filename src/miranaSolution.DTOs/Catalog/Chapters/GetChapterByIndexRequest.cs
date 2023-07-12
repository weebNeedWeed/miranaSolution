namespace miranaSolution.DTOs.Catalog.Chapters;

public record GetChapterByIndexRequest(
    int BookId,
    int ChapterIndex);
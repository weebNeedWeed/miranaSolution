namespace miranaSolution.DTOs.Catalog.Chapters;

public record CreateChapterRequest(
    int BookId,
    string Name,
    int WordCount,
    string Content);
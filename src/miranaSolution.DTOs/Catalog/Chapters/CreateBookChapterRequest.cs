namespace miranaSolution.DTOs.Catalog.Chapters;

public record CreateBookChapterRequest(
    int BookId,
    string Name,
    int WordCount,
    string Content);
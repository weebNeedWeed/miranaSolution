namespace miranaSolution.DTOs.Core.Chapters;

public record CreateBookChapterRequest(
    int BookId,
    string Name,
    int WordCount,
    string Content);
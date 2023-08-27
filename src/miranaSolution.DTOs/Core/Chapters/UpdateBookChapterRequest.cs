namespace miranaSolution.DTOs.Core.Chapters;

public record UpdateBookChapterRequest(
    int BookId,
    int CurrentIndex,
    string Name,
    int WordCount,
    string Content,
    int NewIndex);
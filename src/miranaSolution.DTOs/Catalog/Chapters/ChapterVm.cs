namespace miranaSolution.DTOs.Catalog.Chapters;

public record ChapterVm(
    int Id,
    int Index,
    string Name,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    int ReadCount,
    int WordCount,
    string Content,
    bool HasNextChapter,
    bool HasPreviousChapter);
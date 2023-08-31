namespace miranaSolution.DTOs.Core.Chapters;

public record LatestChapterVm(
    int Id,
    string BookName,
    string BookSlug,
    string Genre,
    int ChapterIndex,
    string ChapterName,
    DateTime UpdatedAt,
    string AuthorName);
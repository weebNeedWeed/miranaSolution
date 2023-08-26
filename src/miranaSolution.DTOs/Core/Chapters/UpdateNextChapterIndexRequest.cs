namespace miranaSolution.DTOs.Core.Chapters;

public record UpdateNextChapterIndexRequest(
    int BookId,
    int CurrentIndex,
    int? NextIndex);
namespace miranaSolution.DTOs.Core.Chapters;

public record UpdatePreviousChapterIndexRequest(
    int BookId,
    int CurrentIndex,
    int? PreviousIndex);
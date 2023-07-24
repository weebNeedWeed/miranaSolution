using miranaSolution.DTOs.Core.Chapters;

namespace miranaSolution.API.ViewModels.Books;

public record ApiGetLatestCreatedChaptersResponse(List<ChapterVm> Chapters);
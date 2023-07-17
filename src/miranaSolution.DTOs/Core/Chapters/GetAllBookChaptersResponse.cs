using miranaSolution.DTOs.Common;

namespace miranaSolution.DTOs.Core.Chapters;

public record GetAllBookChaptersResponse(
    PagerResponse PagerResponse,
    List<ChapterVm> ChapterVms);
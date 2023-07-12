using miranaSolution.DTOs.Common;

namespace miranaSolution.DTOs.Catalog.Chapters;

public record GetAllBookChaptersResponse(
    PagerResponse PagerResponse,
    List<ChapterVm> ChapterVms);
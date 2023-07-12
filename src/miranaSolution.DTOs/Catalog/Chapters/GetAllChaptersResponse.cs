using miranaSolution.DTOs.Common;

namespace miranaSolution.DTOs.Catalog.Chapters;

public record GetAllChaptersResponse(
    PagerResponse PagerResponse,
    List<ChapterVm> ChapterVms);
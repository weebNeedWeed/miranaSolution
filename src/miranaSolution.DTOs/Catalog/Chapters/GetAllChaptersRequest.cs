using miranaSolution.DTOs.Common;

namespace miranaSolution.DTOs.Catalog.Chapters;

public record GetAllChaptersRequest(
    int BookId,
    PagerRequest PagerRequest
    );
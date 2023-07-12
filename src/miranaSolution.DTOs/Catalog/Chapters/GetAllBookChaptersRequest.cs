using miranaSolution.DTOs.Common;

namespace miranaSolution.DTOs.Catalog.Chapters;

public record GetAllBookChaptersRequest(
    int BookId,
    PagerRequest PagerRequest
    );
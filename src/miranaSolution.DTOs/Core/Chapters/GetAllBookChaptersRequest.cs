using miranaSolution.DTOs.Common;

namespace miranaSolution.DTOs.Core.Chapters;

public record GetAllBookChaptersRequest(
    int BookId,
    PagerRequest PagerRequest
);
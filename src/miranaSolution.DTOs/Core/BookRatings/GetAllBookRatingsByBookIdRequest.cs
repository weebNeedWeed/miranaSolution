using miranaSolution.DTOs.Common;

namespace miranaSolution.DTOs.Core.BookRatings;

public record GetAllBookRatingsByBookIdRequest(
    int BookId,
    Guid? UserId,
    PagerRequest PagerRequest);
using miranaSolution.DTOs.Common;

namespace miranaSolution.DTOs.Core.BookRatings;

public record GetAllBookRatingsByBookIdResponse(
    List<BookRatingVm> BookRatingVms,
    PagerResponse PagerResponse);
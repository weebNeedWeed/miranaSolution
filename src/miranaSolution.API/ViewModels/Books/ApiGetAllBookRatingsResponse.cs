using miranaSolution.DTOs.Core.BookRatings;

namespace miranaSolution.API.ViewModels.Books;

public record ApiGetAllBookRatingsResponse(
    List<BookRatingVm> BookRatings,
    int PageIndex,
    int PageSize,
    int TotalPages,
    int TotalRatings);
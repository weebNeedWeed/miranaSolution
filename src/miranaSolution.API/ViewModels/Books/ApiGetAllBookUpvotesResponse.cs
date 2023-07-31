using miranaSolution.DTOs.Core.BookUpvotes;

namespace miranaSolution.API.ViewModels.Books;

public record ApiGetAllBookUpvotesResponse(
    List<BookUpvoteVm> BookUpvotes);
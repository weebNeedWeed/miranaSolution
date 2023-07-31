namespace miranaSolution.DTOs.Core.BookUpvotes;

public record GetAllBookUpvotesResponse(
    List<BookUpvoteVm> BookUpvoteVms);
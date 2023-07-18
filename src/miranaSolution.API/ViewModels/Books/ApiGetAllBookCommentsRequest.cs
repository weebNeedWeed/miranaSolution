namespace miranaSolution.API.ViewModels.Books;

public record ApiGetAllBookCommentsRequest(
    int PageIndex,
    int PageSize);
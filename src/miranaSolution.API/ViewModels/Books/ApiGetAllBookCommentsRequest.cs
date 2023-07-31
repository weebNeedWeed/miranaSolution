namespace miranaSolution.API.ViewModels.Books;

public record ApiGetAllBookCommentsRequest(
    int? ParentId,
    bool? Asc,
    int PageIndex = 1,
    int PageSize = 10);
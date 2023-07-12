namespace miranaSolution.API.ViewModels.Books;

public record ApiGetAllBooksRequest(
    string Keyword,
    string GenreIds,
    bool? IsDone,
    int PageIndex,
    int PageSize);
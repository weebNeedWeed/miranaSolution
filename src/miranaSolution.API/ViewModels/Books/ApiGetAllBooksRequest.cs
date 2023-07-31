namespace miranaSolution.API.ViewModels.Books;

public record ApiGetAllBooksRequest(
    string? Keyword,
    string? GenreIds,
    bool? IsDone,
    int PageIndex = 1,
    int PageSize = 10);
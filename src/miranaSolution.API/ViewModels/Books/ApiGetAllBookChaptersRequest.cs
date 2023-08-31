namespace miranaSolution.API.ViewModels.Books;

public record ApiGetAllBookChaptersRequest(
    bool Detailed = false,
    int PageIndex = 1,
    int PageSize = 10);
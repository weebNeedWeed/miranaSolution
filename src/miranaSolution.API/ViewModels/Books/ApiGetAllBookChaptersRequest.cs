using Microsoft.AspNetCore.Mvc;

namespace miranaSolution.API.ViewModels.Books;

public record ApiGetAllBookChaptersRequest(
    int PageIndex = 1,
    int PageSize = 10);
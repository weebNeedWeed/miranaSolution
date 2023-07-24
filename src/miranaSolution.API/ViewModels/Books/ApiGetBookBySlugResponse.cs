using miranaSolution.DTOs.Core.Books;

namespace miranaSolution.API.ViewModels.Books;

public record ApiGetBookBySlugResponse(
    BookVm Book,
    int TotalUpvotes);
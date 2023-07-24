using miranaSolution.DTOs.Core.Books;

namespace miranaSolution.API.ViewModels.Books;

public record ApiGetRecommendedBooksResponse(
    List<BookVm> Books);
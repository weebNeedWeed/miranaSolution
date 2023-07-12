using miranaSolution.DTOs.Common;

namespace miranaSolution.DTOs.Catalog.Books;

public record GetAllBooksResponse(
    List<BookVm> BookVms,
    PagerResponse PagerResponse);
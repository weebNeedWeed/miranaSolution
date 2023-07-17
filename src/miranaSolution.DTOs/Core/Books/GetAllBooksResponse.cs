using miranaSolution.DTOs.Common;

namespace miranaSolution.DTOs.Core.Books;

public record GetAllBooksResponse(
    List<BookVm> BookVms,
    PagerResponse PagerResponse);
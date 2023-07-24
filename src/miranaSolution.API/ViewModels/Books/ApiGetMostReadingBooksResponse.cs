using miranaSolution.DTOs.Core.Books;

namespace miranaSolution.API.ViewModels.Books;

public record ApiGetMostReadingBooksResponse(List<BookVm> Books);
using miranaSolution.DTOs.Core.Books;

namespace miranaSolution.DTOs.Core.Authors;

public record GetAllBooksByAuthorIdResponse(
    List<BookVm> BookVms);
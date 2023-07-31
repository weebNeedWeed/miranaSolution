using miranaSolution.Data.Entities;
using miranaSolution.DTOs.Core.Bookmarks;
using miranaSolution.DTOs.Core.Books;

namespace miranaSolution.API.ViewModels.Authors;

public record ApiGetAllBooksByAuthorIdResponse(
    List<BookVm> Books);
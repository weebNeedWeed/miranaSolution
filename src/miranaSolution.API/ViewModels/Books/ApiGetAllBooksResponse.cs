using System.ComponentModel.DataAnnotations;
using miranaSolution.DTOs.Core.Books;

namespace miranaSolution.API.ViewModels.Books;

public record ApiGetAllBooksResponse(
    List<BookVm> Books,
    int PageIndex,
    int PageSize,
    int TotalPages);
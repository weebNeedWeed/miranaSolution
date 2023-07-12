using System.ComponentModel.DataAnnotations;
using miranaSolution.DTOs.Catalog.Books;

namespace miranaSolution.API.ViewModels.Books;

public record ApiGetAllBooksResponse(
    [Display(Name = "Books")]
    List<BookVm> BookVms,
    int PageIndex,
    int PageSize,
    int TotalPages);
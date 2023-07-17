using System.ComponentModel.DataAnnotations;
using miranaSolution.DTOs.Core.Chapters;

namespace miranaSolution.API.ViewModels.Books;

public record ApiGetAllBookChaptersResponse(
    [Display(Name = "Chapters")]
    List<ChapterVm> ChapterVms,
    int PageIndex,
    int PageSize,
    int TotalPages);
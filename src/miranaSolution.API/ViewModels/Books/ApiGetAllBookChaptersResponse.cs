using System.ComponentModel.DataAnnotations;
using miranaSolution.DTOs.Core.Chapters;

namespace miranaSolution.API.ViewModels.Books;

public record ApiGetAllBookChaptersResponse(
    List<ChapterVm> Chapters,
    int PageIndex,
    int PageSize,
    int TotalPages,
    int TotalChapters);
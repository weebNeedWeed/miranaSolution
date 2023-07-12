using Microsoft.AspNetCore.Mvc;
using miranaSolution.DTOs.Catalog.Chapters;

namespace miranaSolution.API.ViewModels.Books;

public record ApiCreateBookChapterRequest(
    string Name,
    int WordCount,
    string Content);
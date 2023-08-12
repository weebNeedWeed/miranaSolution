namespace miranaSolution.Admin.ViewModels.Common;

public record PagerResult(
    int PageIndex,
    int PageSize,
    int TotalPages);
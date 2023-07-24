using miranaSolution.DTOs.Core.Authors;

namespace miranaSolution.API.ViewModels.Authors;

public record ApiGetAllAuthorsResponse(
    List<AuthorVm> Authors);
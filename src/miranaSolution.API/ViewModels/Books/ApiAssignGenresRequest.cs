using miranaSolution.DTOs.Common;

namespace miranaSolution.API.ViewModels.Books;

public record ApiAssignGenresRequest(List<CheckboxItem> GenreCheckboxItems);
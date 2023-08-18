using miranaSolution.DTOs.Common;

namespace miranaSolution.DTOs.Core.Books;

public record AssignGenresRequest(
    int BookId,
    List<CheckboxItem> GenreCheckboxItems);
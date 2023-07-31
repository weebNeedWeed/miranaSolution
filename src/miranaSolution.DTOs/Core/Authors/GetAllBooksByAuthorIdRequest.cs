namespace miranaSolution.DTOs.Core.Authors;

public record GetAllBooksByAuthorIdRequest(
    int AuthorId,
    int NumberOfBooks);
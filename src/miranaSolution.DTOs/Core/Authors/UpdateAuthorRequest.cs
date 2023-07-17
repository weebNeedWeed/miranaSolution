namespace miranaSolution.DTOs.Core.Authors;

public record UpdateAuthorRequest(
    int AuthorId,
    string Name,
    string Slug);
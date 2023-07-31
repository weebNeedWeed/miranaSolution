namespace miranaSolution.API.ViewModels.Books;

public record ApiGetAllBookRatingsRequest(
    Guid? UserId,
    int PageIndex = 1,
    int PageSize = 10);
namespace miranaSolution.API.ViewModels.Books;

public record ApiCreateBookRatingRequest(
    string Content,
    int Star);
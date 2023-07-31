namespace miranaSolution.API.ViewModels.Books;

public record ApiUpdateBookRatingRequest(
    string Content,
    int Star);
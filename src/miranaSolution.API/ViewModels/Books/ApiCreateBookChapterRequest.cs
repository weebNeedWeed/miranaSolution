namespace miranaSolution.API.ViewModels.Books;

public record ApiCreateBookChapterRequest(
    string Name,
    int WordCount,
    string Content);
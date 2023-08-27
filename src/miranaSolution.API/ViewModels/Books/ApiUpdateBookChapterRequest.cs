namespace miranaSolution.API.ViewModels.Books;

public record ApiUpdateBookChapterRequest(
    string Name,
    int WordCount,
    string Content,
    int NewIndex);
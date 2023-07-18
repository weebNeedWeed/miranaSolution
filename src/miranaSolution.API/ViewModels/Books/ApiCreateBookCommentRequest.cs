namespace miranaSolution.API.ViewModels.Books;

public record ApiCreateBookCommentRequest(
    string Content,
    int? ParentId);
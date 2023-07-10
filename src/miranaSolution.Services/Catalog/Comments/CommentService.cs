using miranaSolution.DTOs.Catalog.Comments;

namespace miranaSolution.Services.Catalog.Comments;

public class CommentService : ICommentService
{
    public Task<CommentDto> AddComment(int bookId, CreateCommentRequest request)
    {
        throw new NotImplementedException();
    }
}
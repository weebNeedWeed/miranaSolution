using miranaSolution.DTOs.Catalog.Comments;

namespace miranaSolution.Services.Catalog.Comments;

public interface ICommentService
{
    Task<CommentDto> AddComment(int bookId, CreateCommentRequest request);
}
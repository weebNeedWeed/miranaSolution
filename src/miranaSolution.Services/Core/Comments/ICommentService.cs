using miranaSolution.DTOs.Core.Comments;

namespace miranaSolution.Services.Core.Comments;

public interface ICommentService
{
    Task<CreateBookCommentResponse> CreateBookCommentAsync(CreateBookCommentRequest request);

    Task DeleteBookCommentAsync(DeleteBookCommentRequest request);

    Task<GetBookCommentByIdResponse> GetBookCommentById(GetBookCommentByIdRequest request);

    Task<CountCommentByUserIdResponse> CountCommentByUserIdAsync(CountCommentByUserIdRequest request);

    Task<GetAllBookCommentsResponse> GetAllBookCommentsAsync(GetAllBookCommentsRequest request);
}
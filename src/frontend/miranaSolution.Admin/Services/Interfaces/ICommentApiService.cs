using miranaSolution.API.ViewModels.Books;
using miranaSolution.API.ViewModels.Common;
using Refit;

namespace miranaSolution.Admin.Services.Interfaces;

public interface ICommentApiService
{
    [Get("/api/books/{id}/comments")]
    Task<ApiResult<ApiGetAllBookCommentsResponse>> GetAllCommentsByBookIdAsync(
        [AliasAs("id")] int id,
        [Query] ApiGetAllBookCommentsRequest request);
    
    [Delete("/api/books/{bookId}/comments/{commentId}:force")]
    Task<ApiResult<dynamic>> DeleteCommentAsync(
        [AliasAs("bookId")] int bookId,
        [AliasAs("commentId")] int commentId);
}
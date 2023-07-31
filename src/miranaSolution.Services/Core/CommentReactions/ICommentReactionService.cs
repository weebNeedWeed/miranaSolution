using miranaSolution.DTOs.Core.CommentReactions;

namespace miranaSolution.Services.Core.CommentReactions;

public interface ICommentReactionService
{
    Task<CreateCommentReactionResponse> CreateCommentReactionAsync(CreateCommentReactionRequest request);

    Task DeleteCommentReactionAsync(DeleteCommentReactionRequest request);

    Task<CountCommentReactionByCommentIdResponse> CountCommentReactionByCommentIdAsync(CountCommentReactionByCommentIdRequest request);
    
    Task<CountCommentReactionByUserIdResponse> CountCommentReactionByUserIdAsync(CountCommentReactionByUserIdRequest request);

    Task<CheckUserIsReactedResponse> CheckUserIsReactedAsync(CheckUserIsReactedRequest request);
}
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using miranaSolution.API.ViewModels.Comments;
using miranaSolution.API.ViewModels.Common;
using miranaSolution.DTOs.Core.CommentReactions;
using miranaSolution.Services.Core.CommentReactions;
using miranaSolution.Services.Exceptions;

namespace miranaSolution.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CommentsController : ControllerBase
{
    private readonly ICommentReactionService _commentReactionService;
    
    public CommentsController(ICommentReactionService commentReactionService)
    {
        _commentReactionService = commentReactionService;
    }
    
    [HttpPost("{commentId:int}/reaction")]
    public async Task<IActionResult> CreateCommentReaction([FromRoute] int commentId)
    {
        var userId = GetUserIdFromClaim();
        try
        {
            var createCommentReactionResponse = await _commentReactionService.CreateCommentReactionAsync(
                new CreateCommentReactionRequest(userId, commentId));

            return Ok(new ApiSuccessResult<CommentReactionVm>(createCommentReactionResponse.CommentReactionVm));
        }
        catch (UserAlreadyReactsToCommentException ex)
        {
            return Ok(new ApiErrorResult(ex.Message));
        }
    }
    
    [HttpDelete("{commentId:int}/reaction")]
    public async Task<IActionResult> DeleteCommentReaction([FromRoute] int commentId)
    {
        var userId = GetUserIdFromClaim();
        try
        {
            await _commentReactionService.DeleteCommentReactionAsync(
                new DeleteCommentReactionRequest(userId, commentId));

            return Ok(new ApiSuccessResult<object>());
        }
        catch (UserNotReactedToCommentException ex)
        {
            return Ok(new ApiErrorResult(ex.Message));
        }
    }

    [HttpGet("{commentId:int}/reaction/counting")]
    [AllowAnonymous]
    public async Task<IActionResult> CountReactionByCommentId([FromRoute] int commentId)
    {
        try
        {
            var countReactionByCommentIdResponse = await _commentReactionService.CountCommentReactionByCommentIdAsync(
                new CountCommentReactionByCommentIdRequest(commentId));
            var totalReactions = countReactionByCommentIdResponse.TotalReactions;

            var response = new ApiCountReactionByCommentIdResponse(totalReactions);

            return Ok(new ApiSuccessResult<ApiCountReactionByCommentIdResponse>(response));
        }
        catch (CommentNotFoundException ex)
        {
            return Ok(new ApiErrorResult(ex.Message));
        }
    }

    [HttpGet("{commentId:int}/reaction")]
    public async Task<IActionResult> CheckUserIsReacted([FromRoute] int commentId)
    {
        var userId = GetUserIdFromClaim();
        
        try
        {
            var checkUserIsReactedResponse = await _commentReactionService.CheckUserIsReactedAsync(
                new CheckUserIsReactedRequest(
                    userId, commentId));
            var isReacted = checkUserIsReactedResponse.IsReacted;
            var response = new ApiCheckUserIsReactedResponse(isReacted);

            return Ok(new ApiSuccessResult<ApiCheckUserIsReactedResponse>(response));
        }
        catch (CommentNotFoundException ex)
        {
            return Ok(new ApiErrorResult(ex.Message));
        }
    }
    
    private Guid GetUserIdFromClaim()
    {
        string userId = User.Claims.First(x => x.Type == JwtRegisteredClaimNames.Sid).Value;
        return new Guid(userId);
    }
}
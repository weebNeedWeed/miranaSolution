﻿using Microsoft.EntityFrameworkCore;
using miranaSolution.Data.Entities;
using miranaSolution.Data.Main;
using miranaSolution.DTOs.Authentication.Users;
using miranaSolution.DTOs.Core.CommentReactions;
using miranaSolution.Services.Authentication.Users;
using miranaSolution.Services.Exceptions;

namespace miranaSolution.Services.Core.CommentReactions;

public class CommentReactionService : ICommentReactionService
{
    private readonly IUserService _userService;
    private readonly MiranaDbContext _context;

    public CommentReactionService(IUserService userService, MiranaDbContext context)
    {
        _userService = userService;
        _context = context;
    }

    public async Task<CreateCommentReactionResponse> CreateCommentReactionAsync(CreateCommentReactionRequest request)
    {
        var getUserByIdResponse = await _userService.GetUserByIdAsync(
            new GetUserByIdRequest(request.UserId));
        if (getUserByIdResponse.UserVm is null)
        {
            throw new UserNotFoundException("The user with given Id does not exist.");
        }
        
        var comment = await _context.Comments.FindAsync(request.CommentId);
        if (comment is null)
        {
            throw new CommentNotFoundException("The comment with given Id does not exist.");
        }

        var commentReaction = new CommentReaction
        {
            CommentId = request.CommentId,
            UserId = request.UserId
        };

        await _context.CommentReactions.AddAsync(commentReaction);
        await _context.SaveChangesAsync();

        var response = new CreateCommentReactionResponse(
            new CommentReactionVm(request.UserId, request.CommentId));
        return response;
    }

    public async Task DeleteCommentReactionAsync(DeleteCommentReactionRequest request)
    {
        var getUserByIdResponse = await _userService.GetUserByIdAsync(
            new GetUserByIdRequest(request.UserId));
        if (getUserByIdResponse.UserVm is null)
        {
            throw new UserNotFoundException("The user with given Id does not exist.");
        }
        
        var comment = await _context.Comments.FindAsync(request.CommentId);
        if (comment is null)
        {
            throw new CommentNotFoundException("The comment with given Id does not exist.");
        }

        var commentReaction = new CommentReaction
        {
            UserId = request.UserId,
            CommentId = request.CommentId
        };

        _context.CommentReactions.Remove(commentReaction);
        await _context.SaveChangesAsync();
    }

    public async Task<CountCommentReactionByCommentIdResponse> CountCommentReactionByCommentIdAsync(CountCommentReactionByCommentIdRequest request)
    {
        var comment = await _context.Comments.FindAsync(request.CommentId);
        if (comment is null)
        {
            throw new CommentNotFoundException("The comment with given Id does not exist.");
        }

        var totalReactions = await _context.CommentReactions.CountAsync(x => x.CommentId == request.CommentId);

        return new CountCommentReactionByCommentIdResponse(totalReactions);
    }

    public async Task<CountCommentReactionByUserIdResponse> CountCommentReactionByUserIdAsync(CountCommentReactionByUserIdRequest request)
    {
        var getUserByIdResponse = await _userService.GetUserByIdAsync(
            new GetUserByIdRequest(request.UserId));
        if (getUserByIdResponse.UserVm is null)
        {
            throw new UserNotFoundException("The user with given Id does not exist.");
        }
        
        var totalReactions = await _context.CommentReactions.CountAsync(x => x.UserId == request.UserId);

        return new CountCommentReactionByUserIdResponse(totalReactions);
    }
}
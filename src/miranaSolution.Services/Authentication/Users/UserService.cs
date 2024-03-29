﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using miranaSolution.Data.Entities;
using miranaSolution.DTOs.Authentication.Roles;
using miranaSolution.DTOs.Authentication.Users;
using miranaSolution.DTOs.Common;
using miranaSolution.Services.Exceptions;
using miranaSolution.Services.Systems.Images;
using miranaSolution.Services.Systems.JwtTokenGenerators;
using miranaSolution.Services.Validations;
using miranaSolution.Utilities.Constants;

namespace miranaSolution.Services.Authentication.Users;

public class UserService : IUserService
{
    private readonly IImageSaver _imageSaver;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly IValidatorProvider _validatorProvider;
    private readonly RoleManager<AppRole> _roleManager;

    public UserService(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        IJwtTokenGenerator jwtTokenGenerator, IImageSaver imageSaver, IValidatorProvider validatorProvider, RoleManager<AppRole> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtTokenGenerator = jwtTokenGenerator;
        _imageSaver = imageSaver;
        _validatorProvider = validatorProvider;
        _roleManager = roleManager;
    }

    /// <exception cref="UserAlreadyExistsException">
    ///     Thrown when the user with given Email or UserName already exists
    /// </exception>
    public async Task<RegisterUserResponse> RegisterUserAsync(RegisterUserRequest request)
    {
        _validatorProvider.Validate(request);

        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is not null) throw new UserAlreadyExistsException("The user with given Email already exists.");

        user = await _userManager.FindByNameAsync(request.UserName);
        if (user is not null) throw new UserAlreadyExistsException("The user with given UserName already exists.");

        user = new AppUser
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            UserName = request.UserName
        };

        await _userManager.CreateAsync(user, request.Password);
        user = await _userManager.FindByNameAsync(user.UserName);

        await _userManager.AddToRoleAsync(user, RolesConstant.User);
        
        var userVm = MapUserIntoUserVm(user);

        var response = new RegisterUserResponse(userVm);
        return response;
    }

    /// <exception cref="UserNotFoundException">
    ///     Thrown when the user with given UserName does not exist
    /// </exception>
    /// <exception cref="InvalidCredentialException">
    ///     Thrown when the request's password does not match the found user's password
    /// </exception>
    public async Task<AuthenticateUserResponse> AuthenticateUserAsync(AuthenticateUserRequest request)
    {
        _validatorProvider.Validate(request);

        var user = await _userManager.FindByNameAsync(request.UserName);
        if (user is null) throw new UserNotFoundException("The user with given User Name does not exists.");

        var signInResult = await _signInManager.PasswordSignInAsync(
            request.UserName,
            request.Password,
            false,
            false);

        if (!signInResult.Succeeded) throw new InvalidCredentialException("Invalid credentials.");

        var userVm = MapUserIntoUserVm(user);

        var token = await _jwtTokenGenerator.GenerateTokenAsync(user);
        var response = new AuthenticateUserResponse(userVm, token);

        return response;
    }

    public async Task<GetUserByUserNameResponse> GetUserByUserNameAsync(GetUserByUserNameRequest request)
    {
        var user = await _userManager.FindByNameAsync(request.UserName);
        if (user is null) return new GetUserByUserNameResponse(null);

        var userVm = MapUserIntoUserVm(user);

        var response = new GetUserByUserNameResponse(userVm);
        return response;
    }

    public async Task<GetUserByEmailResponse> GetUserByEmailAsync(GetUserByEmailRequest request)
    {
        var user = await _userManager.FindByNameAsync(request.Email);
        if (user is null) return new GetUserByEmailResponse(null);

        var userVm = MapUserIntoUserVm(user);

        var response = new GetUserByEmailResponse(userVm);
        return response;
    }

    /// <exception cref="UserNotFoundException">
    ///     Thrown when the user with given User Name does not exist
    /// </exception>
    public async Task<UpdateUserProfileResponse> UpdateUserProfileAsync(UpdateUserProfileRequest request)
    {
        _validatorProvider.Validate(request);

        var user = await _userManager.FindByNameAsync(request.UserName);
        if (user is null) throw new UserNotFoundException("The user with given Id does not exists.");

        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.Email = request.Email;

        if (request.Avatar is not null)
        {
            await _imageSaver.DeleteImageIfExistAsync(user.Avatar);
            user.Avatar = await _imageSaver.SaveImageAsync(request.Avatar, request.AvatarExtension!);
        }

        await _userManager.UpdateAsync(user);

        var userVm = MapUserIntoUserVm(user);

        var response = new UpdateUserProfileResponse(userVm);
        return response;
    }

    /// <exception cref="UserNotFoundException">
    ///     Thrown when the user with given User Name does not exist
    /// </exception>
    /// <exception cref="InvalidCredentialException">
    ///     Thrown when the request's password does not match the current password of the user
    /// </exception>
    public async Task<UpdateUserPasswordResponse> UpdateUserPasswordAsync(UpdateUserPasswordRequest request)
    {
        _validatorProvider.Validate(request);

        var user = await _userManager.FindByNameAsync(request.UserName);
        if (user is null) throw new UserNotFoundException("The user with given Id does not exists.");

        if (!await _userManager.CheckPasswordAsync(user, request.CurrentPassword))
            throw new InvalidCredentialException("Invalid current password.");

        await _userManager.ChangePasswordAsync(
            user, request.CurrentPassword, request.NewPassword);

        var userVm = MapUserIntoUserVm(user);

        var response = new UpdateUserPasswordResponse(userVm);
        return response;
    }

    public async Task<GetUserByIdResponse> GetUserByIdAsync(GetUserByIdRequest request)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user is null) return new GetUserByIdResponse(null);

        var userVm = MapUserIntoUserVm(user);

        var response = new GetUserByIdResponse(userVm);
        return response;
    }

    public async Task DeleteUserAsync(DeleteUserRequest request)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user is null) throw new UserNotFoundException("The user with given Id does not exist.");

        if (!string.IsNullOrEmpty(user.Avatar)) await _imageSaver.DeleteImageIfExistAsync(user.Avatar);

        await _userManager.DeleteAsync(user);
    }

    public async Task<IncreaseReadBookCountBy1Response> IncreaseReadBookCountBy1Async(IncreaseReadBookCountBy1Request request)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user is null) throw new UserNotFoundException("The user with given Id does not exist.");

        user.ReadBookCount++;
        await _userManager.UpdateAsync(user);

        return new IncreaseReadBookCountBy1Response(user.ReadBookCount);
    }

    public async Task<IncreaseReadChapterCountBy1Response> IncreaseReadChapterCountBy1Async(IncreaseReadChapterCountBy1Request request)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user is null) throw new UserNotFoundException("The user with given Id does not exist.");

        user.ReadChapterCount++;
        await _userManager.UpdateAsync(user);

        return new IncreaseReadChapterCountBy1Response(user.ReadChapterCount);
    }

    public async Task<GetAllUsersResponse> GetAllUsersAsync(GetAllUsersRequest request)
    {
        var query = _userManager.Users;
        if (request.Keyword is not null)
        {
            query = query.Where(
                x => x.FirstName.Contains(request.Keyword)
                     || x.LastName.Contains(request.Keyword)
                     || x.Email.Contains(request.Keyword));
        }

        var totalRecords = await query.CountAsync();
        var pageSize = request.PagerRequest.PageSize;
        var pageIndex = request.PagerRequest.PageIndex;

        query = query.Skip((pageIndex - 1) * pageSize)
            .Take(pageSize);

        var users = await query.ToListAsync();
        var userVms = users.Select(MapUserIntoUserVm).ToList();
        var pagerResponse = new PagerResponse(
            pageIndex, pageSize, totalRecords);
        
        return new GetAllUsersResponse(userVms, pagerResponse);
    }

    public async Task AssignRolesAsync(AssignRolesRequest request)
    {
        _validatorProvider.Validate(request);
        
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user is null)
        {
            throw new UserNotFoundException("The user with given Id does not exist.");
        }

        foreach (var item in request.RoleCheckboxItems)
        {
            if (!await _roleManager.RoleExistsAsync(item.Label))
            {
                continue;
            }
            
            if (item.IsChecked)
            {
                if (!await _userManager.IsInRoleAsync(user, item.Label))
                {
                    await _userManager.AddToRoleAsync(user, item.Label);
                } 
            }
            else
            {
                if (await _userManager.IsInRoleAsync(user, item.Label))
                {
                    await _userManager.RemoveFromRoleAsync(user, item.Label);
                }
            }
        }
    }

    public async Task<GetRolesByUserIdResponse> GetRolesByUserIdAsync(GetRolesByUserIdRequest request)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user is null)
        {
            throw new UserNotFoundException("The user with given Id does not exist.");
        }

        var roles = await _userManager.GetRolesAsync(user);
        var roleVms = roles.ToList().Select(x => new RoleVm(
            Guid.NewGuid(),
            x,
            "")).ToList();

        return new GetRolesByUserIdResponse(roleVms);
    }

    private UserVm MapUserIntoUserVm(AppUser user)
    {
        var userVm = new UserVm(
            user.Id,
            user.FirstName,
            user.LastName,
            user.UserName,
            user.Email,
            user.Avatar,
            user.ReadBookCount,
            user.ReadChapterCount);

        return userVm;
    }
}
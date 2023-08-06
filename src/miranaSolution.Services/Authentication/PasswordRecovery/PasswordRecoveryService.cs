using System.Text;
using System.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using miranaSolution.Data.Entities;
using miranaSolution.Data.Main;
using miranaSolution.DTOs.Authentication.PasswordRecovery;
using miranaSolution.DTOs.Systems.Mails;
using miranaSolution.Services.Exceptions;
using miranaSolution.Services.Systems.Mails;
using miranaSolution.Services.Validations;

namespace miranaSolution.Services.Authentication.PasswordRecovery;

public class PasswordRecoveryService : IPasswordRecoveryService
{
    private readonly IValidatorProvider _validatorProvider;
    private readonly MiranaDbContext _context;
    private readonly UserManager<AppUser> _userManager;
    private readonly IMailService _mailService;

    public PasswordRecoveryService(IValidatorProvider validatorProvider, UserManager<AppUser> userManager, IMailService mailService, MiranaDbContext context)
    {
        _validatorProvider = validatorProvider;
        _userManager = userManager;
        _mailService = mailService;
        _context = context;
    }

    public async Task SendRecoveryEmailAsync(SendRecoveryEmailRequest request)
    {
        _validatorProvider.Validate(request);

        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            throw new UserNotFoundException("The user with given email does not exist");
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        if (token is null)
        {
            throw new Exception("Something has gone wrong.");
        }

        token = HttpUtility.UrlEncode(token);
        var email = HttpUtility.UrlEncode(request.Email);

        var emailBodyBuilder = new StringBuilder();
        emailBodyBuilder.Append(
            "Hello, we are here to help you recover your password. Please click the following link and follow the instruction to create a new password for your account.\n");
        emailBodyBuilder.Append($"{request.Callback}?email={email}&token={token}");

        var mailRequest = new EmailRequest(
            request.Email,
            "Mirana Online Reader account's password recovery!",
            emailBodyBuilder.ToString());

        await _mailService.SendEmailAsync(mailRequest);
    }

    public async Task RedeemTokenAsync(RedeemTokenRequest request)
    {
        _validatorProvider.Validate(request);
        
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            throw new UserNotFoundException("The user with given email does not exist");
        }

        var recoveryResult = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
        if (!recoveryResult.Succeeded)
        {
            throw new InvalidTokenException("The given token is invalid.");
        }
    }
}
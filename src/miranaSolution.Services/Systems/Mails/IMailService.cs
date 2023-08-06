using miranaSolution.DTOs.Systems.Mails;

namespace miranaSolution.Services.Systems.Mails;

public interface IMailService
{
    Task SendEmailAsync(EmailRequest request);
}
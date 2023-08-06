using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Microsoft.Extensions.Options;
using miranaSolution.DTOs.Systems.Mails;

namespace miranaSolution.Services.Systems.Mails;

public class SESService : IMailService
{
    private readonly IAmazonSimpleEmailService _amazonSimpleEmailService;
    private readonly MailOptions _mailOptions;

    public SESService(IAmazonSimpleEmailService amazonSimpleEmailService, IOptions<MailOptions> mailOptions)
    {
        _amazonSimpleEmailService = amazonSimpleEmailService;
        _mailOptions = mailOptions.Value;
    }

    public async Task SendEmailAsync(EmailRequest request)
    {
        var subject = new Content(request.Subject);
        var body = new Body(new Content(request.Body));
        var message = new Message(subject, body);

        var destination = new Destination(
            new List<string> { request.Receiver });

        var sendEmailRequest = new SendEmailRequest(_mailOptions.Source, destination, message);

        await _amazonSimpleEmailService.SendEmailAsync(sendEmailRequest);
    }
}